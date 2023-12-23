﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MMKiwi.CBindingSG.SourceGenerator;

public static class HandleGenerationHelper
{
    internal static string GenerateExtensionClass(HandleGenerator.GenerationInfo.Ok genInfo)
    {
        StringBuilder resFile = new();

        Stack<TypeDeclarationSyntax> parentClasses = [];
        Stack<BaseNamespaceDeclarationSyntax> parentNamespaces = [];

        string className = genInfo.ClassSymbol.Identifier.ToString();

        SyntaxNode? parent = genInfo.ClassSymbol;

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
                    foreach (var use in cus.Usings)
                        resFile.AppendLine(use.ToString());
                    break;
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

        if (genInfo.GenerateConstruct)
        {
            if (genInfo.BaseHandleType is HandleGenerator.BaseType.Parameterless)
            {
                resFile.AppendLine($$"""
                                     
                                        {{Constants.SgAttribute}}
                                     #if NET_7_0_OR_GREATER
                                         static {{className}} {{Constants.IConstructableHandle}}<{{className}}>.Construct(bool ownsHandle) => new();
                                     #else
                                         public static {{className}} Construct(bool ownsHandle) => new();
                                     #endif
                                     """);
            }
            else if (genInfo.BaseHandleType is HandleGenerator.BaseType.Bool)
            {
                resFile.AppendLine($$"""

                                         {{Constants.SgAttribute}}
                                     #if NET_7_0_OR_GREATER
                                         static {{className}} {{Constants.IConstructableHandle}}<{{className}}>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
                                     #else
                                         public static {{className}} Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
                                     #endif
                                     """);
            }
        }

        if (genInfo.GenerateOwns)
        {
            resFile.AppendLine($$"""
                                     
                                     {{Constants.SgAttribute}}
                                     public class Owns() : {{className}}(true);
                                 """);
        }

        if (genInfo.GenerateDoesntOwn)
        {
            resFile.AppendLine($$"""
                                     
                                     {{Constants.SgAttribute}}
                                     public class DoesntOwn() : {{className}}(false);
                                 """);
        }

        if (genInfo.GenerateConstructor)
        {
            resFile.AppendLine($$"""
                                 
                                     {{Constants.SgAttribute}}
                                     {{genInfo.ConstructorVisibility}} {{className}}(bool ownsHandle): base(ownsHandle) { }
                                 """);
        }

        for(int i = 0; i < parentClasses.Count + parentNamespaces.Count; i++)
            resFile.AppendLine("}");

        return resFile.ToString();
    }
}