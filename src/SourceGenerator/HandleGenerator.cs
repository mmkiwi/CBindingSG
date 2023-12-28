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

namespace MMKiwi.CBindingSG.SourceGenerator;

[Generator]
public class HandleGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Do a simple filter for methods
        IncrementalValuesProvider<GenerationInfo> methodDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                Constants.HandleMarkerFullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,// select methods with attributes
                transform: GetClasses);// sect the methods with the [CbsgWrapperMethod] attribute

        // Combine the selected methods with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<GenerationInfo>)> compilationAndMethods
            = context.CompilationProvider.Combine(methodDeclarations.Collect());

        // Generate the source using the compilation and methods
        context.RegisterSourceOutput(compilationAndMethods,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static GenerationInfo GetClasses(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        // we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)context.TargetNode;
        INamedTypeSymbol classSymbol = (INamedTypeSymbol)context.TargetSymbol;

        AttributeData attribute = context.Attributes[0];

        bool staticVirtual = context.SemanticModel.Compilation.SupportsRuntimeCapability(RuntimeCapability.VirtualStaticsInInterfaces);
        
        bool needsConstructMethod = false;
        bool hasConstructMethod = false;
        bool generateOwns = true;
        bool generateDoesntOwn = true;
        bool needsOwns = false;
        bool needsDoesntOwn = false;
        bool hasOwns = false;
        bool hasDoesntOwn = false;

        bool isPartial = classSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

        if (!isPartial)
        {
            //If the method is not partial, skip everything else and just report an error. 
            return new GenerationInfo.ErrorNotPartial
            {
                ClassSymbol = classSyntax,
            };
        }

        MemberVisibility constructorVisibility = MemberVisibility.Protected;

        foreach (KeyValuePair<string, TypedConstant> namedArgument in attribute.NamedArguments)
        {
            switch (namedArgument)
            {
                case { Key: nameof(CbsgGenerateHandleAttribute.ConstructorVisibility), Value.Value: int cv }:
                    constructorVisibility = (MemberVisibility)cv;
                    break;
                case { Key: nameof(CbsgGenerateHandleAttribute.GenerateOwns), Value.Value: bool bv }:
                    generateOwns = bv;
                    break;
                case { Key: nameof(CbsgGenerateHandleAttribute.GenerateDoesntOwn), Value.Value: bool bv }:
                    generateDoesntOwn = bv;
                    break;
            }
        }

        BaseType baseFound = BaseType.NotFound;

        List<string> parentClasses = [];

        var parentClass = classSymbol.BaseType;
        while (parentClass != null)
        {
            string ds = parentClass.ToDisplayString();
            parentClasses.Add(ds);
            if (ds == "System.Runtime.InteropServices.SafeHandle")
            {
                needsDoesntOwn = needsOwns = true;
                needsConstructMethod = parentClass.Constructors.Any(c => c.Parameters.Length == 0);
                baseFound = GetConstructorType(classSymbol, classSymbol.BaseType!);
                break;
            }

            parentClass = parentClass.BaseType;
        }

        if (baseFound == BaseType.NotFound)
            return new GenerationInfo.ErrorBadBase()
            {
                ClassSymbol = classSyntax, ParentClass = parentClasses
            };

        if ((needsOwns && generateOwns) || (needsDoesntOwn && generateDoesntOwn))
        {
            foreach (var member in classSymbol.GetMembers().OfType<INamedTypeSymbol>())
            {
                switch (member.Name)
                {
                    case "Owns":
                        hasOwns = true;
                        break;
                    case "DoesntOwn":
                        hasDoesntOwn = true;
                        break;
                }
            }
        }

        foreach (var baseInterface in classSymbol.Interfaces.Where(baseInterface
                     => baseInterface.IsGenericType &&
                        baseInterface.ConstructedFrom.ToDisplayString()
                            .StartsWith($"{Constants.IConstructableHandleFullName}<")))
        {
            needsConstructMethod = true;
            foreach (IMethodSymbol symbol in baseInterface.GetMembers().OfType<IMethodSymbol>())
            {
                if (symbol != null)
                    hasConstructMethod |= classSymbol.FindImplementationForInterfaceMember(symbol) is not null;
            }
        }


        bool hasConstructor = classSymbol.Constructors.Any(constructor =>
            constructor.Parameters is
            [
                { Type.SpecialType: SpecialType.System_Boolean }
            ]);

        return new GenerationInfo.Ok
        {
            ClassSymbol = classSyntax,
            GenerateConstruct = needsConstructMethod && !hasConstructMethod,
            BaseHandleType = baseFound,
            StaticVirtual = staticVirtual,
            GenerateConstructor =
                !hasConstructor && baseFound == BaseType.Bool &&
                constructorVisibility != MemberVisibility.DoNotGenerate,
            ConstructorVisibility = constructorVisibility.ToStringFast(),
            IsSealedOrAbstract = classSymbol.IsAbstract || classSymbol.IsSealed,
            GenerateOwns = baseFound != BaseType.Parameterless && needsOwns && generateOwns && !hasOwns,
            GenerateDoesntOwn = baseFound != BaseType.Parameterless && needsDoesntOwn && generateDoesntOwn && !hasDoesntOwn
        };
    }
    private static BaseType GetConstructorType(INamedTypeSymbol currClass, INamedTypeSymbol baseClass)
    {
        if (SearchConstructors(currClass, out BaseType type))
        {
            return type;
        }
        if (SearchConstructors(baseClass, out type))
        {
            return type;
        }
        
        return BaseType.Unknown;

        bool SearchConstructors(INamedTypeSymbol namedTypeSymbol, out BaseType type)
        {

            foreach (var constructor in namedTypeSymbol.Constructors)
            {
                if (constructor.DeclaringSyntaxReferences.Length == 0)
                    // This is an autogenerated constructor, skip 
                    continue;
            
                switch (constructor.Parameters)
                {
                    case []:
                        type = BaseType.Parameterless;
                        return true;
                    case [{ Type.SpecialType: SpecialType.System_Boolean }]:
                        type = BaseType.Bool;
                        return true;
                }
            }
            type = BaseType.Unknown; 
            return false;
        }
    }

    
    
    static void Execute(Compilation compilation, ImmutableArray<GenerationInfo> classes,
        SourceProductionContext context)
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
                case GenerationInfo.ErrorBadBase ebb:
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag11ImplementHandle,
                        cls.ClassSymbol.GetLocation(),
                        cls.ClassSymbol.Identifier, string.Join(", ", ebb.ParentClass)));
                    break;
                case GenerationInfo.ErrorNotPartial:
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag02IsNotPartial,
                        cls.ClassSymbol.GetLocation(),
                        [cls.ClassSymbol.Identifier, "Class"]));
                    break;
                case GenerationInfo.Ok genInfo:
                    {
                        if (!genInfo.IsSealedOrAbstract)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Constants.Diag10SealAbstract,
                                cls.ClassSymbol.GetLocation(),
                                cls.ClassSymbol.Identifier));
                        }

                        // generate the source code and add it to the output
                        string result = HandleGenerationHelper.GenerateExtensionClass(genInfo);
                        context.AddSource($"Construct.{cls.ClassSymbol.ToFullDisplayName()}.g.cs",
                            SourceText.From(result, Encoding.UTF8));
                        break;
                    }
            }
        }
    }

    public abstract class GenerationInfo
    {
        public required ClassDeclarationSyntax ClassSymbol { get; init; }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return ClassSymbol.GetHashCode();
        }

        public class ClassNameEqualityComparer : EqualityComparer<GenerationInfo>
        {
            public new static ClassNameEqualityComparer Default => new();

            [ExcludeFromCodeCoverage]
            public override bool Equals(GenerationInfo x, GenerationInfo y) =>
                x.ClassSymbol.ToFullDisplayName() == y.ClassSymbol.ToFullDisplayName();

            [ExcludeFromCodeCoverage]
            public override int GetHashCode(GenerationInfo obj) => obj.ClassSymbol.ToFullDisplayName().GetHashCode();
        }

        public class ErrorNotPartial : GenerationInfo;

        public class ErrorBadBase : GenerationInfo
        {
            public required IEnumerable<string> ParentClass { get; init; }
        }

        public class Ok : GenerationInfo
        {
            public required bool GenerateConstructor { get; init; }
            public required bool GenerateConstruct { get; init; }
            public required BaseType BaseHandleType { get; init; }
            public required string ConstructorVisibility { get; init; }
            public required bool IsSealedOrAbstract { get; init; }
            public required bool GenerateOwns { get; init; }
            public required bool GenerateDoesntOwn { get; init; }
            
            public required bool StaticVirtual { get; init; }
        }
    }

    public enum BaseType
    {
        NotFound,
        Bool,
        Parameterless,
        Unknown
    }
}