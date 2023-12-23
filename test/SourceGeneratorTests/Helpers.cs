// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

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

    public static SyntaxTree TestHandleBase => GetResource("TestHandleBase");
    public static SyntaxTree TestError => GetResource("TestError");
    public static SyntaxTree SampleHandles => GetResource("SampleHandles");

    public static SyntaxTree GetResource(string key) => CSharpSyntaxTree.ParseText(TestResources.ResourceManager.GetString(key)!);
}