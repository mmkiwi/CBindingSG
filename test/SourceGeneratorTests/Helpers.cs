// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Versioning;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Xunit.Abstractions;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

internal static class Helpers
{
    public const string AssemblyName = "MMKiwi.CBindingSG.SourceGeneratorTests.GeneratedSourceTest";

    public static GeneratorDriver OutputDiagnostics(this (GeneratorDriver, Compilation) result, ITestOutputHelper output)
    {
        var diagnostics = result.Item2.GetDiagnostics().Where(d =>
            d.Severity switch
            {
                DiagnosticSeverity.Hidden => false,
                DiagnosticSeverity.Info => false,
                DiagnosticSeverity.Warning => true,
                DiagnosticSeverity.Error => true,
                _ => true
            }).ToList();

        output.WriteLine($"THERE ARE {diagnostics.Count} DIAGNOSTICS");
        foreach (var diag in diagnostics)
            output.WriteLine(diag.ToString());

        return result.Item1;
    }

    public static SyntaxTree Global => CSharpSyntaxTree.ParseText("""
                                                                  global using global::System;
                                                                  global using global::System.Collections.Generic;
                                                                  global using global::System.IO;
                                                                  global using global::System.Linq;

                                                                  """ + TestResources.ResourceManager.GetString("Global")!);

    public static SyntaxTree TestHandleBase(TargetFramework f) => GetResource("TestHandleBase", f);
    public static SyntaxTree TestHandle(TargetFramework f) => GetResource("TestHandle", f);

    public static SyntaxTree TestError => GetResource("TestError", TargetFramework.Net80);
    public static SyntaxTree TestWrapper(TargetFramework f) => GetResource("TestWrapper", f);

    public static SyntaxTree SampleHandles => GetResource("SampleHandles", TargetFramework.Net80);

    public static SyntaxTree GetResource(string key, TargetFramework f) => CSharpSyntaxTree.ParseText(TestResources.ResourceManager.GetString(key)!, options:new CSharpParseOptions(preprocessorSymbols: f.ToPreprocessor()));
    public static ImmutableArray<string> ExcludedResults => ["IConstructableWrapper.g.cs", "IHasHandle.g.cs", "IConstructableHandle.g.cs", "CbsgConstructionHelper.g.cs"];
}