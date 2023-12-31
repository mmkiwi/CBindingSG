﻿// This Source Code Form is subject to the terms of the Mozilla Public
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
public class InteropGenerator : IIncrementalGenerator
{
    private bool GenerateAll { get; }
    public InteropGenerator() : this(false)
    {

    }
    public InteropGenerator(bool generateAll)
    {
        GenerateAll = generateAll;
    }
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {


        // Do a simple filter for methods
        IncrementalValuesProvider<MethodGenerationInfo> methodDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(Constants.WrapperMarkerFullName,
                predicate: (node, _) => node is MethodDeclarationSyntax,// select methods with attributes
                transform: GetMethodToGenerate)// sect the methods with the [CbsgWrapperMethod] attribute
            .Where(static m => m is not null)!;// filter out attributed methods that we don't care about

        // Combine the selected methods with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<MethodGenerationInfo>)> compilationAndMethods
            = context.CompilationProvider.Combine(methodDeclarations.Collect());

        // Generate the source using the compilation and methods
        context.RegisterSourceOutput(compilationAndMethods, static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static MethodGenerationInfo? GetMethodToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        // we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        IMethodSymbol methodSymbol = (IMethodSymbol)context.TargetSymbol;
        MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)context.TargetNode;

        // There will only be one
        var attributeData = context.Attributes.First();

        string methodName = methodSymbol.Name;

        foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
        {
            if (namedArgument.Key == "MethodName" && namedArgument.Value.Value?.ToString() is { } ns)
            {
                methodName = ns;
            }
        }

        string? errorMethod = FindErrorMethod(methodSymbol);

        return new MethodGenerationInfo(methodSyntax, methodName, errorMethod);

    }
    private static string? FindErrorMethod(IMethodSymbol methodSymbol)
    {
        if (CheckAttributeList(methodSymbol.GetAttributes(), out string? result))
            return result;

        ISymbol? parent = methodSymbol.ContainingSymbol;
        while (parent is not null)
        {
            if (CheckAttributeList(parent.GetAttributes(), out result))
                return result;
            parent = parent.ContainingSymbol;
        }
        return result;

        bool CheckAttributeList(ImmutableArray<AttributeData> attributes, out string? s)
        {
            foreach (AttributeData attribute in attributes)
            {
                if (attribute.AttributeClass?.ToDisplayString() == Constants.ErrorMarkerFullName
                    && attribute.ConstructorArguments is
                    [
                        { Kind: TypedConstantKind.Type },
                        { Kind: TypedConstantKind.Primitive, Type: { SpecialType: SpecialType.System_String } }
                    ])
                {
                    INamedTypeSymbol typeArg = (INamedTypeSymbol)attribute.ConstructorArguments[0].Value!;
                    string methodArg = attribute.ConstructorArguments[1].Value!.ToString();

                    s = $"{typeArg.ToDisplayString()}.{methodArg}";
                    return true;
                }
            }
            s = null;
            return false;
        }
    }

    static void Execute(Compilation compilation, ImmutableArray<MethodGenerationInfo> methods, SourceProductionContext context)
    {
        if (methods.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        var semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees.First());
        INamespaceSymbol? sysNamespace = semanticModel.LookupNamespacesAndTypes(0, null, "System").OfType<INamespaceSymbol>().FirstOrDefault();
        ITypeSymbol? argNullException = semanticModel.LookupSymbols(0, sysNamespace, "ArgumentNullException").OfType<INamedTypeSymbol>().FirstOrDefault();

        CheckForArgNull(argNullException);


        bool hasThrowIfNull = argNullException.GetMembers().Any(m => m.Name == "ThrowIfNull");

        IEnumerable<IGrouping<TypeDeclarationSyntax?, MethodGenerationInfo>> distinctClasses = methods.GroupBy(GetParentClass);

        foreach (var cls in distinctClasses)
        {
            if (cls.Key is null)
            {
                foreach (var method in cls)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag01ParentNotFound,
                        method.Method.GetLocation(),
                        method.Method.ToDiagString()));
                }
            }
            else
            {
                // generate the source code and add it to the output
                string result = InteropGenerationHelper.GenerateExtensionClass(compilation, cls!, context, hasThrowIfNull);
                context.AddSource($"InteropGenerator.{cls.Key.ToFullDisplayName()}.g.cs", SourceText.From(result, Encoding.UTF8));
            }
        }

        return;

        [ExcludeFromCodeCoverage]
        void CheckForArgNull([NotNull] ITypeSymbol? t)
        {
            // This should never be true unless the .net runtime isn't referenced at all
            if (t is null)
                throw new Exception("Could not find System.ArgumentException class");
        }

        static TypeDeclarationSyntax? GetParentClass(MethodGenerationInfo method)
        {
            var parent = method.Method.Parent;
            while (parent is not null or CompilationUnitSyntax)
            {
                if (parent is TypeDeclarationSyntax parentType)
                    return parentType;
                parent = parent.Parent;
            }
            return null;
        }
    }


    [ExcludeFromCodeCoverage]
    public record MethodGenerationInfo(MethodDeclarationSyntax Method, string TargetName, string? ErrorMethod);
}