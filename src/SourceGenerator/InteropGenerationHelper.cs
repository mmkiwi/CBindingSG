// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MMKiwi.CBindingSG.SourceGenerator;

public static class InteropGenerationHelper
{
    internal static string GenerateExtensionClass(Compilation compilation,
        IGrouping<TypeDeclarationSyntax, InteropGenerator.MethodGenerationInfo> classGroup, SourceProductionContext context, bool hasThrowIfNull)
    {
        StringBuilder resFile = new();

        Stack<TypeDeclarationSyntax> parentClasses = [];
        Stack<BaseNamespaceDeclarationSyntax> parentNamespaces = [];

        TypeDeclarationSyntax parentClass = classGroup.Key;
        SyntaxNode? parent = classGroup.Key;

        while (parent is not null)
        {
            switch (parent)
            {
                case BaseNamespaceDeclarationSyntax ns:
                    parentNamespaces.Push(ns);
                    break;
                case TypeDeclarationSyntax cls:
                    parentClasses.Push(cls);
                    break;
                case CompilationUnitSyntax cus:
                    {
                        foreach (var use in cus.Usings)
                            resFile.AppendLine(use.ToString());
                        break;
                    }
            }

            parent = parent.Parent;
        }

        resFile.AppendLine("#nullable enable");

        foreach (var ns in parentNamespaces)
        {
            resFile.AppendLine($$"""namespace {{ns.Name}} {""");
        }

        foreach (var cls in parentClasses)
        {
            resFile.AppendLine($$"""
                                 {{cls.Modifiers}} {{cls.Keyword}} {{cls.Identifier}}
                                 {
                                 """);
        }

        foreach (var methodInfo in classGroup)
        {
            var method = methodInfo.Method;
            if (!method.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PartialKeyword)))
            {
                context.ReportDiagnostic(Diagnostic.Create(Constants.Diag02IsNotPartial,
                    method.GetLocation(),
                    method.ToDiagString(), "Method"));
                continue;
            }

            //Find the raw method to call
            MethodTransformations? interopMethod = FindInteropMethod(parentClass, methodInfo, compilation, context);

            if (interopMethod == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(Constants.Diag03CouldNotGenerateWrapper,
                    method.GetLocation(),
                    method.ToDiagString()));
                resFile.AppendLine($$"""
                                         {{Constants.SgAttribute}}
                                         {{method.Modifiers}} {{method.ReturnType}} {{method.Identifier}}{{method.ParameterList.RemoveAttributes()}}
                                         {
                                             throw new NotImplementedException();
                                         }
                                     """);
                continue;
            }

            resFile.AppendLine($$"""
                                     {{Constants.SgAttribute}}
                                     {{method.Modifiers}} {{method.ReturnType}} {{method.Identifier}}{{method.ParameterList.RemoveAttributes()}}
                                     {
                                 {{GenerateMethod(method, interopMethod, methodInfo.ErrorMethod, hasThrowIfNull)}}
                                     }
                                 """);
        }

        for (int i = 0; i < parentClasses.Count + parentNamespaces.Count; i++)
        {
            resFile.AppendLine("}");
        }

        return resFile.ToString();
    }

    private static string GenerateMethod(MethodDeclarationSyntax method, MethodTransformations interopMethod, string? errorMethod, bool hasThrowIfNull)
    {
        const string space = "        ";
        StringBuilder methodString = new();

        if (interopMethod.Return == TransformType.WrapperOut)
        {
            methodString.AppendLine($"{space}{method.ReturnType} __return_value = null!;");
            methodString.AppendLine($"{space}{interopMethod.InteropMethod.ReturnType} __return_value_raw;");
        }
        else if (interopMethod.Return == TransformType.Direct)
        {
            methodString.AppendLine($"{space}{method.ReturnType} __return_value;");
        }

        foreach (var param in interopMethod.Parameters)
        {
            if (param.TransformType is TransformType.WrapperIn or TransformType.WrapperExplicitIn)
            {
                if (param.WrapperParam.Type is NullableTypeSyntax)
                {
                    methodString.AppendLine($"{space}{param.InteropParam.Type} __param_{param.WrapperParam.Identifier} = ({param.WrapperParam.Identifier} as {Constants.IHasHandle}<{param.InteropParam.Type}>)?.Handle ?? {Constants.Namespace}.{Constants.ConstructHelperClass}.GetNullHandle<{param.InteropParam.Type}>();");
                }
                else
                {
                    if (hasThrowIfNull)
                    {
                        methodString.AppendLine($"{space}ArgumentNullException.ThrowIfNull({param.WrapperParam.Identifier});");
                    }
                    else
                    {
                        methodString.AppendLine($"{space}if({param.WrapperParam.Identifier} is null)");
                        methodString.AppendLine($"{space}  throw new ArgumentNullException(\"{param.WrapperParam.Identifier}\");");
                    }
                    methodString.AppendLine($"{space}{param.InteropParam.Type} __param_{param.WrapperParam.Identifier} = (({Constants.IHasHandle}<{param.InteropParam.Type}>){param.WrapperParam.Identifier}).Handle;");
                }
            }
            else if (param.TransformType == TransformType.WrapperOut)
            {
                methodString.AppendLine($"{space}{param.InteropParam.Type} __out_{param.InteropParam.Identifier}_raw;");
            }
            else if (param.TransformType == TransformType.WrapperRef)
            {
                if (param.WrapperParam.Type is NullableTypeSyntax)
                {
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Type} __ref_{param.InteropParam.Identifier}_raw = ({param.WrapperParam.Identifier} as {Constants.IHasHandle}<{param.InteropParam.Type}>)?.Handle ?? {Constants.Namespace}.{Constants.ConstructHelperClass}.GetNullHandle<{param.InteropParam.Type}>();");
                }
                else
                {
                    if (hasThrowIfNull)
                    {
                        methodString.AppendLine($"{space}ArgumentNullException.ThrowIfNull({param.WrapperParam.Identifier});");
                    }
                    else
                    {
                        methodString.AppendLine($"{space}if({param.WrapperParam.Identifier} is null)");
                        methodString.AppendLine($"{space}  throw new ArgumentNullException(\"{param.WrapperParam.Identifier}\");");
                    }
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Type} __ref_{param.InteropParam.Identifier}_raw = (({Constants.IHasHandle}<{param.InteropParam.Type}>){param.WrapperParam.Identifier}).Handle;");
                }
            }
        }

        methodString.Append(space);
        if (interopMethod.Return == TransformType.Direct)
        {
            methodString.Append(" __return_value =");
        }

        else if (interopMethod.Return == TransformType.WrapperOut)
        {
            methodString.Append(" __return_value_raw =");
        }

        methodString.Append($" {interopMethod.InteropMethod.Identifier}(");

        bool isFirst = true;
        foreach (var param in interopMethod.Parameters)
        {
            if (!isFirst)
            {
                methodString.Append(", ");
            }

            isFirst = false;
            if (param.TransformType == TransformType.WrapperIn)
            {
                methodString.Append($"__param_{param.WrapperParam.Identifier}");
            }
            if (param.TransformType == TransformType.WrapperExplicitIn)
            {
                methodString.Append($"in __param_{param.WrapperParam.Identifier}");
            }
            else if (param.TransformType == TransformType.Direct)
            {
                methodString.Append($"{param.WrapperParam.Identifier}");
            }
            else if (param.TransformType == TransformType.DirectOut)
            {
                methodString.Append($"out {param.WrapperParam.Identifier}");
            }
            else if (param.TransformType == TransformType.DirectIn)
            {
                methodString.Append($"in {param.WrapperParam.Identifier}");
            }
            else if (param.TransformType == TransformType.DirectRef)
            {
                methodString.Append($"ref {param.WrapperParam.Identifier}");
            }
            else if (param.TransformType == TransformType.WrapperOut)
            {
                methodString.Append($"out __out_{param.InteropParam.Identifier}_raw");
            }
            else if (param.TransformType == TransformType.WrapperRef)
            {
                methodString.Append($"ref __ref_{param.InteropParam.Identifier}_raw");
            }
        }

        methodString.AppendLine(");");

        if (!string.IsNullOrEmpty(errorMethod))
            methodString.AppendLine($"{space}global::{errorMethod}();");

        foreach (var param in interopMethod.Parameters)
        {
            if (param.TransformType == TransformType.WrapperOut)
            {
                if (param.WrapperParam.Type is NullableTypeSyntax nts)
                {
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Identifier} =  {Constants.Namespace}.{Constants.ConstructHelperClass}.ConstructNullable<{nts.ElementType}, {param.InteropParam.Type}>(__out_{param.InteropParam.Identifier}_raw);");
                }
                else
                {
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Identifier} =  {Constants.Namespace}.{Constants.ConstructHelperClass}.Construct<{param.WrapperParam.Type}, {param.InteropParam.Type}>(__out_{param.InteropParam.Identifier}_raw);");
                }
            }
            else if (param.TransformType == TransformType.WrapperRef)
            {
                if (param.WrapperParam.Type is NullableTypeSyntax nts)
                {
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Identifier} =  {Constants.Namespace}.{Constants.ConstructHelperClass}.ConstructNullable<{nts.ElementType}, {param.InteropParam.Type}>(__ref_{param.InteropParam.Identifier}_raw);");
                }
                else
                {
                    methodString.AppendLine(
                        $"{space}{param.InteropParam.Identifier} =  {Constants.Namespace}.{Constants.ConstructHelperClass}.Construct<{param.WrapperParam.Type}, {param.InteropParam.Type}>(__ref_{param.InteropParam.Identifier}_raw);");
                }
            }
        }

        if (interopMethod.Return == TransformType.Direct)
        {
            methodString.AppendLine($"{space}return __return_value;");
        }

        else if (interopMethod.Return == TransformType.WrapperOut)
        {
            if (method.ReturnType is NullableTypeSyntax nts)
                methodString.AppendLine(
                    $"{space}__return_value = {Constants.Namespace}.{Constants.ConstructHelperClass}.ConstructNullable<{nts.ElementType}, {interopMethod.InteropMethod.ReturnType}>(__return_value_raw);");
            else
                methodString.AppendLine(
                    $"{space}__return_value = {Constants.Namespace}.{Constants.ConstructHelperClass}.Construct<{method.ReturnType}, {interopMethod.InteropMethod.ReturnType}>(__return_value_raw);");
            methodString.AppendLine($"{space}return __return_value;");
        }

        return methodString.ToString();
    }

    private static bool CheckForLibraryImport(MethodDeclarationSyntax candidateSibling, Compilation compilation)
    {
        foreach (var att in candidateSibling.AttributeLists.SelectMany(attList => attList.Attributes))
        {
            IMethodSymbol attributeSymbol = (IMethodSymbol)compilation.GetSemanticModel(candidateSibling.SyntaxTree)
                .GetSymbolInfo(att).Symbol!;

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            string fullName = attributeContainingTypeSymbol.ToDisplayString();

            if (fullName is "System.Runtime.InteropServices.LibraryImportAttribute" or "System.Runtime.InteropServices.DllImportAttribute")
                return true;
        }

        return false;
    }

    private static MethodTransformations? FindInteropMethod(TypeDeclarationSyntax parentClass,
        InteropGenerator.MethodGenerationInfo methodInfo, Compilation compilation, SourceProductionContext context)
    {
        var wrapperMethod = methodInfo.Method;
        //For now, name must be the same. TODO: Add parameter to attribute to override
        foreach (MethodDeclarationSyntax candidateInterop in parentClass.Members.OfType<MethodDeclarationSyntax>())
        {
            if (candidateInterop.Identifier.ToFullString() != methodInfo.TargetName)// Name must match
                continue;

            if (candidateInterop.IsEquivalentTo(wrapperMethod))// Cant match on itself
                continue;

            if (candidateInterop.ParameterList.Parameters.Count !=
                wrapperMethod.ParameterList.Parameters.Count)// Parameter count must be the same
            {
                context.ReportDiagnostic(Diagnostic.Create(Constants.Diag04SkipParameters,
                    wrapperMethod.GetLocation(),
                    wrapperMethod.ToDiagString(),
                    candidateInterop.ToDiagString()));
                continue;
            }

            if (!CheckForLibraryImport(candidateInterop, compilation))// Must have [LibraryImport]
            {
                context.ReportDiagnostic(Diagnostic.Create(Constants.Diag05SkipLibraryImport,
                    wrapperMethod.GetLocation(),
                    wrapperMethod.ToDiagString(), candidateInterop.ToDiagString()));
                continue;
            }

            ImmutableArray<ParameterCompatibility> parameters =
                IterateParameters(wrapperMethod, compilation, candidateInterop);
            var invalidParameters = parameters.Where(p => p.TransformType is TransformType.Invalid);
            if (InvalidParametersDiag(invalidParameters, wrapperMethod, candidateInterop, context))
            {
                continue;
            }

            var returnCompatibility = CheckReturn(wrapperMethod, candidateInterop, compilation);
            if (returnCompatibility is not TransformType.Invalid)
            {
                return new MethodTransformations(candidateInterop, parameters, returnCompatibility);
            }

            context.ReportDiagnostic(Diagnostic.Create(Constants.Diag07SkipMatchReturn,
                wrapperMethod.GetLocation(),
                wrapperMethod.ToDiagString(), candidateInterop.ToDiagString()));
            continue;

            static bool InvalidParametersDiag(IEnumerable<ParameterCompatibility> invalidParameters,
                MethodDeclarationSyntax wrapperMethod, MethodDeclarationSyntax candidateInterop,
                SourceProductionContext context)
            {
                foreach (var invalidParam in invalidParameters)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Constants.Diag06SkipParameter,
                        wrapperMethod.GetLocation(),
                        wrapperMethod.ToDiagString(), invalidParam.InteropParam.Identifier,
                        candidateInterop.ToDiagString()));
                    return true;
                }

                return false;
            }
        }

        return null;
    }

    private static TransformType CheckReturn(MethodDeclarationSyntax wrapperMethod,
        MethodDeclarationSyntax candidateInterop, Compilation compilation)
    {
        if (wrapperMethod.ReturnType is PredefinedTypeSyntax predefined &&
            predefined.Keyword.IsKind(SyntaxKind.VoidKeyword))
            return TransformType.Void;

        ITypeSymbol wrapperTypeSymbol = (ITypeSymbol)compilation.GetSemanticModel(
                wrapperMethod.ReturnType.SyntaxTree,
                true)
            .GetSymbolInfo(wrapperMethod.ReturnType).Symbol!;

        ITypeSymbol interopTypeSymbol = (ITypeSymbol)compilation.GetSemanticModel(
                candidateInterop.ReturnType.SyntaxTree,
                true)
            .GetSymbolInfo(candidateInterop.ReturnType).Symbol!;

        if (wrapperTypeSymbol.Equals(interopTypeSymbol, SymbolEqualityComparer.Default))
            return TransformType.Direct;

        List<ITypeSymbol> hierarchy = [interopTypeSymbol];

        var parent = interopTypeSymbol.BaseType;
        while (parent is not null)
        {
            hierarchy.Add(parent);
            parent = parent.BaseType;
        }

        foreach (var handleType in wrapperTypeSymbol.Interfaces.Where(i => i.Name is Constants.IConstructableWrapper))
        {
            if (hierarchy.Contains(handleType.TypeArguments[1]))
            {
                return TransformType.WrapperOut;
            }
        }

        return TransformType.Invalid;
    }

    private static ImmutableArray<ParameterCompatibility> IterateParameters(MethodDeclarationSyntax wrapperMethod,
        Compilation compilation, MethodDeclarationSyntax candidateInterop)
    {
        var parameters =
            ImmutableArray.CreateBuilder<ParameterCompatibility>(wrapperMethod.ParameterList.Parameters.Count);

        //walk through each parameter
        for (int i = 0; i < wrapperMethod.ParameterList.Parameters.Count; i++)
        {
            parameters.Add(CheckParameterCompatibility(wrapperMethod.ParameterList.Parameters[i],
                candidateInterop.ParameterList.Parameters[i], compilation));
        }

        return parameters.ToImmutable();
    }

    private static ParameterCompatibility CheckParameterCompatibility(ParameterSyntax wrapperParam,
        ParameterSyntax interopParam, Compilation compilation)
    {
        if (compilation.GetSemanticModel(wrapperParam.Type!.SyntaxTree, true).GetSymbolInfo(wrapperParam.Type).Symbol is
            not ITypeSymbol wrapperTypeSymbol)
            return new(TransformType.Invalid, interopParam, wrapperParam);

        if (compilation.GetSemanticModel(interopParam.Type!.SyntaxTree, true).GetSymbolInfo(interopParam.Type).Symbol is
            not ITypeSymbol interopTypeSymbol)
            return new(TransformType.Invalid, interopParam, wrapperParam);

        if (wrapperTypeSymbol.Equals(interopTypeSymbol, SymbolEqualityComparer.Default))
        {
            return new(wrapperParam.Modifiers switch
            {
                [{ RawKind: (int)SyntaxKind.RefKeyword }] => TransformType.DirectRef,
                [{ RawKind: (int)SyntaxKind.OutKeyword }] => TransformType.DirectOut,
                [{ RawKind: (int)SyntaxKind.InKeyword }] => TransformType.DirectIn,
                _ => TransformType.Direct,
            }, interopParam, wrapperParam);
        }

        foreach (var handleType in wrapperTypeSymbol.Interfaces.Where(i => i.Name == Constants.IHasHandle))
        {
            List<ITypeSymbol> hierarchy =
            [
                interopTypeSymbol
            ];

            var parent = interopTypeSymbol.BaseType;
            while (parent is not null)
            {
                hierarchy.Add(parent);
                parent = parent.BaseType;
            }

            if (hierarchy.Contains(handleType.TypeArguments[0]))
            {
                return wrapperParam.Modifiers switch
                {
                    [{ RawKind: (int)SyntaxKind.RefKeyword }] => new(TransformType.WrapperRef, interopParam,
                        wrapperParam),
                    [{ RawKind: (int)SyntaxKind.OutKeyword }] => new(TransformType.WrapperOut, interopParam,
                        wrapperParam),
                    [{ RawKind: (int)SyntaxKind.InKeyword }] => new(TransformType.WrapperExplicitIn, interopParam,
                        wrapperParam),
                    _ => new(TransformType.WrapperIn, interopParam, wrapperParam)
                };
            }
        }

        return new(TransformType.Invalid, interopParam, wrapperParam);
    }

    [ExcludeFromCodeCoverage]
    private record MethodTransformations(
        MethodDeclarationSyntax InteropMethod,
        ImmutableArray<ParameterCompatibility> Parameters,
        TransformType Return);

    [ExcludeFromCodeCoverage]
    private record ParameterCompatibility(
        TransformType TransformType,
        ParameterSyntax InteropParam,
        ParameterSyntax WrapperParam);

    private enum TransformType
    {
        Invalid,
        Void,
        Direct,
        DirectOut,
        DirectIn,
        DirectRef,
        WrapperIn,
        WrapperRef,
        WrapperOut,
        WrapperExplicitIn
    }
}