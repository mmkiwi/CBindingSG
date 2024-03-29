﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MMKiwi.CBindingSG.SourceGenerator;

public static class WrapperGenerationHelper
{
    internal static string GenerateExtensionClass(WrapperGenerator.GenerationInfo.Ok genInfo)
    {
        StringBuilder resFile = new();

        Stack<TypeDeclarationSyntax> parentClasses = [];
        Stack<BaseNamespaceDeclarationSyntax> parentNamespaces = [];

        SyntaxNode? parent = genInfo.ClassSyntax;

        while (parent is not null)
        {
            if (parent is BaseNamespaceDeclarationSyntax ns)
                parentNamespaces.Push(ns);
            else if (parent is TypeDeclarationSyntax cls)
            {
                parentClasses.Push(cls);
            }

            if (parent is CompilationUnitSyntax cus)
            {
                foreach (var use in cus.Usings)
                    resFile.AppendLine(use.ToString());
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

        string wrapperType = genInfo.WrapperType.Split('.').Last();

        switch (genInfo)
        {
            case { GenerateExplicitConstruct: true, HasImplicitConstruct: true }:
                resFile.AppendLine($"""
                                    
                                        {Constants.SgAttribute}
                                        static global::{genInfo.WrapperType} global::{Constants.IConstructableWrapperFullName}<global::{genInfo.WrapperType}, global::{genInfo.HandleType}>.Construct(global::{genInfo.HandleType} handle) => global::{genInfo.WrapperType}.Construct(handle);
                                    """);
                break;
            case { GenerateExplicitConstruct: true, HasImplicitConstruct: false }:
                resFile.AppendLine($"""
                                    
                                        {Constants.SgAttribute}
                                        static global::{genInfo.WrapperType} global::{Constants.IConstructableWrapperFullName}<global::{genInfo.WrapperType}, global::{genInfo.HandleType}>.Construct(global::{genInfo.HandleType} handle) => new(handle);
                                    """);
                break;
            case {GenerateImplicitConstruct:true}:
                resFile.AppendLine($"""
                                    
                                        {Constants.SgAttribute}
                                        static global::{genInfo.WrapperType} Construct(global::{genInfo.HandleType} handle) => new(handle);
                                    """);
                break;
        }
        
        
        if (genInfo.NeedsConstructor)
        {
            resFile.AppendLine($"""

                {Constants.SgAttribute}
                {genInfo.ConstructorVisibility} {wrapperType}(global::{genInfo.HandleType} handle) => Handle = handle;
            """);
        }

        if (genInfo.NeedsImplicitHandle)
        {
            resFile.AppendLine($$"""

                {{Constants.SgAttribute}}
                {{genInfo.HandleVisibility}} global::{{genInfo.HandleType}} Handle { get; {{(genInfo.HandleSetVisibility == nameof(MemberVisibility.DoNotGenerate) ? "" : $"{genInfo.HandleSetVisibility} set; ")}}}
            """);
        }

        if (genInfo.NeedsExplicitHandle)
        {
            resFile.AppendLine($$"""

                {{Constants.SgAttribute}}
                global::{{genInfo.HandleType}} {{Constants.IHasHandle}}<global::{{genInfo.HandleType}}>.Handle => Handle;
            """);
        }

        for (int i = 0; i < parentClasses.Count + parentNamespaces.Count; i++)
        {
            resFile.AppendLine("}");
        }

        return resFile.ToString();
    }
}
