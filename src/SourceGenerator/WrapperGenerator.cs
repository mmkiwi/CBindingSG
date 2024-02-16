// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using NetEscapades.EnumGenerators;

namespace MMKiwi.CBindingSG.SourceGenerator;

[Generator]
public class WrapperGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Do a simple filter for methods
        IncrementalValuesProvider<GenerationInfo> methodDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                Constants.GenWrapperMarkerFullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,// select methods with attributes
                transform: GetMethodsToGenerate);// sect the methods with the [CbsgWrapperMethod] attribute

        // Combine the selected methods with the `Compilation`
        IncrementalValueProvider<ImmutableArray<GenerationInfo>> compilationAndMethods
            = methodDeclarations.Collect();

        // Generate the source using the compilation and methods
        context.RegisterSourceOutput(compilationAndMethods,
            static (spc, source) => Execute(source, spc));
    }

    static GenerationInfo GetMethodsToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        // we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)context.TargetNode;
        INamedTypeSymbol classSymbol = (INamedTypeSymbol)context.TargetSymbol;

        if (!classSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
        {
            return new GenerationInfo.ErrorNotPartial
            {
                ClassSyntax = classSyntax
            };
        }

        bool isConstructable = false;
        
        AttributeData attribute = context.Attributes.First();

        MemberVisibility ctorVisibility = MemberVisibility.Private;
        MemberVisibility handleVisibility = MemberVisibility.Internal;
        MemberVisibility handleSetVisibility = MemberVisibility.DoNotGenerate;

        foreach (KeyValuePair<string, TypedConstant> namedArgument in attribute.NamedArguments)
        {
            switch (namedArgument.Key)
            {
                case "ConstructorVisibility"
                    when namedArgument.Value.Value is int cv:
                    ctorVisibility = (MemberVisibility)cv;
                    break;
                case "HandleVisibility" when namedArgument.Value.Value is int hv:
                    handleVisibility = (MemberVisibility)hv;
                    break;
                case "HandleSetVisibility" when namedArgument.Value.Value is int mh:
                    handleSetVisibility = (MemberVisibility)mh;
                    break;
            }
        }

        string wrapperTypeStr = classSymbol.ToDisplayString();
        ITypeSymbol? handleType = null;

        // Only needed iif the class implements IConstructableWrapper, so start by assuming they aren't needed
        // until that interface is encountered
        ConstructType hasConstructMethod = ConstructType.None;

        // Only needed iif the class implements IHasHandle, so start by assuming they aren't needed
        // until that interface is encountered
        bool hasConstructor = true;
        bool hasExplicitHandle = true;
        bool hasImplicitHandle = true;

        bool neverOwns = false;

        bool hasIDisposable = false;

        foreach (INamedTypeSymbol baseInterface in classSymbol.Interfaces)
        {
            if (baseInterface.IsGenericType &&
                baseInterface.ConstructedFrom.ToDisplayString().StartsWith($"{Constants.IConstructableWrapperFullName}<"))
            {
                handleType = baseInterface.TypeArguments[1];
                isConstructable = true;
                hasConstructMethod = FindConstructMethod(baseInterface, classSymbol, handleType);
            }


            if (baseInterface.IsGenericType &&
                baseInterface.ConstructedFrom.ToDisplayString().StartsWith($"{Constants.IHasHandleFullName}<"))
            {
                // These are only true if it implements IHasHandle
                hasExplicitHandle = false;
                hasConstructor = false;
                hasImplicitHandle = false;

                handleType = baseInterface.TypeArguments[0];

                neverOwns |= handleType.GetAttributes().Any(att => att.AttributeClass?.ToDisplayString() == Constants.NeverOwnsMarkerFullName);

                foreach (var member in Enumerable.OfType<IPropertySymbol>(baseInterface.GetMembers()))
                {
                    // Get handle type
                    hasExplicitHandle |= classSymbol.FindImplementationForInterfaceMember(member) is not null;

                    foreach (var implementedCtor in classSymbol.Constructors)
                    {
                        if (implementedCtor.Parameters.Length == 1 && implementedCtor.Parameters[0].Type
                                .Equals(handleType, SymbolEqualityComparer.Default))
                        {
                            hasConstructor = true;
                        }
                    }

                    foreach (var implementedMember in classSymbol.GetMembers().OfType<IPropertySymbol>())
                    {
                        if (implementedMember.Name == "Handle" &&
                            implementedMember.Type.Equals(handleType, SymbolEqualityComparer.IncludeNullability))
                        {
                            hasImplicitHandle = true;
                        }
                    }
                }
            }

            if (baseInterface.ToDisplayString() == "System.IDisposable")
            {
                hasIDisposable = true;
            }
        }

        if (handleType == null)
        {
            return new GenerationInfo.ErrorDoesNotImplement
            {
                ClassSyntax = classSyntax
            };
        }
        
        bool staticVirtual = context.SemanticModel.Compilation.SupportsRuntimeCapability(RuntimeCapability.VirtualStaticsInInterfaces);
        
        return new GenerationInfo.Ok
        {
            ClassSyntax = classSyntax,
            NeedsConstructor = !hasConstructor && ctorVisibility != MemberVisibility.DoNotGenerate,
            ConstructorVisibility = ctorVisibility.ToStringFast(),
            HandleType = handleType.ToDisplayString(),
            WrapperType = wrapperTypeStr,
            HasImplicitConstruct = hasConstructMethod.HasFlagFast(ConstructType.Implicit),
            GenerateExplicitConstruct = isConstructable && !hasConstructMethod.HasFlagFast(ConstructType.Explicit) && staticVirtual,
            GenerateImplicitConstruct = isConstructable && !hasConstructMethod.HasFlagFast(ConstructType.Implicit) && !staticVirtual,
            NeedsExplicitHandle = !hasExplicitHandle,
            NeedsImplicitHandle = !hasImplicitHandle && handleVisibility != MemberVisibility.DoNotGenerate,
            HandleVisibility = handleVisibility.ToStringFast(),
            HandleSetVisibility = handleSetVisibility.ToStringFast(),
            MissingIDisposable = !neverOwns && !hasIDisposable,
            SupportsVirtualStatic = staticVirtual
        };

        /*
         * child types should reimplement IDisposable
        static bool FindIDisposableInParent(INamedTypeSymbol classSymbol)
        {
            var parent = classSymbol.BaseType;
            while (parent != null)
            {
                if (Enumerable.Any(parent.Interfaces, baseInterface => baseInterface.ToDisplayString() == "System.IDisposable"))
                {
                    return true;
                }

                parent = parent.BaseType;
            }

            return false;
        }
        */
    }
    private static ConstructType FindConstructMethod(INamedTypeSymbol baseInterface, INamedTypeSymbol classSymbol, ITypeSymbol handleType)
    {
        var result = ConstructType.None;

        foreach (var member in baseInterface.GetMembers().OfType<IMethodSymbol>())
        {
            if (classSymbol.FindImplementationForInterfaceMember(member) is IMethodSymbol { ExplicitInterfaceImplementations.Length: > 0 })
            {
                result |= ConstructType.Explicit;
                break;
            }
        }

        foreach (var method in classSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            if (method.Name == "Construct"
                && method.ReturnType.Equals(classSymbol, SymbolEqualityComparer.Default)
                && method.Parameters.Length == 1
                && method.Parameters[0].Type.Equals(handleType, SymbolEqualityComparer.Default))
            {
                result |= ConstructType.Implicit;
                break;
            }
        }

        return result;
    }

    static void Execute(ImmutableArray<GenerationInfo> classes, SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        foreach (GenerationInfo cls in classes.Distinct(GenerationInfo.ClassNameEqualityComparer.Default))
        {
            switch (cls)
            {
                case GenerationInfo.ErrorNotPartial:
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag02IsNotPartial,
                        cls.ClassSyntax.GetLocation(),
                        cls.ClassSyntax.Identifier,
                        "Class"));
                    break;
                case GenerationInfo.ErrorDoesNotImplement:
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag09NoImplement,
                        cls.ClassSyntax.GetLocation(),
                        cls.ClassSyntax.Identifier));
                    break;
                case GenerationInfo.Ok
                {
                    GenerateExplicitConstruct: false,
                    GenerateImplicitConstruct: false,
                    NeedsExplicitHandle: false,
                    NeedsImplicitHandle: false,
                    NeedsConstructor: false,
                }:
                    continue;
                case GenerationInfo.Ok genInfo:
                    {
                        

                        if (genInfo.MissingIDisposable)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Constants.Diag08IDisposable,
                                cls.ClassSyntax.GetLocation(),
                                cls.ClassSyntax.Identifier));
                        }

                        // generate the source code and add it to the output
                        string result = WrapperGenerationHelper.GenerateExtensionClass(genInfo);
                        context.AddSource($"Construct.{cls.ClassSyntax.ToFullDisplayName()}.g.cs",
                            SourceText.From(result, Encoding.UTF8));
                        break;
                    }
            }
        }
    }

    public abstract class GenerationInfo
    {
        public required ClassDeclarationSyntax ClassSyntax { get; init; }

        public class ErrorNotPartial : GenerationInfo;

        public class ErrorDoesNotImplement : GenerationInfo;

        public class Ok : GenerationInfo
        {
            public required string WrapperType { get; init; }
            public required string HandleType { get; init; }
            public required string ConstructorVisibility { get; init; }
            public required bool NeedsConstructor { get; init; }
            
            public required bool SupportsVirtualStatic { get; init; } 

            public required bool HasImplicitConstruct { get; init; }
            public required bool GenerateExplicitConstruct { get; init; }
            public required bool GenerateImplicitConstruct { get; init; }
            public required bool NeedsExplicitHandle { get; init; }
            public required bool NeedsImplicitHandle { get; init; }

            public required string HandleVisibility { get; init; }
            public required string? HandleSetVisibility { get; init; }
            public required bool MissingIDisposable { get; init; }
        }

        public class ClassNameEqualityComparer : EqualityComparer<GenerationInfo>
        {
            public new static ClassNameEqualityComparer Default => new();

            [ExcludeFromCodeCoverage]
            public override bool Equals(GenerationInfo x, GenerationInfo y) =>
                x.ClassSyntax.ToFullDisplayName() == y.ClassSyntax.ToFullDisplayName();

            public override int GetHashCode(GenerationInfo obj) => obj.ClassSyntax.ToFullDisplayName().GetHashCode();
        }
    }

    [Flags, EnumExtensions]
    public enum ConstructType : byte
    {
        None = 0,
        Explicit = 0b0001,
        Implicit = 0b0010
    }
}