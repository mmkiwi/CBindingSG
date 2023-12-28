// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;

using Basic.Reference.Assemblies;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using MMKiwi.CBindingSG.SourceGenerator;

using Xunit.Abstractions;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

[UsesVerify]
public class InteropGeneratorTest
{
    private readonly ITestOutputHelper output;

    public InteropGeneratorTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public Task Driver()
    {
        var driver = GeneratorDriver(default, [], false).OutputDiagnostics(output);

        return Verify(driver).UseDirectory($"snapshots");
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task RunResults(TargetFramework f)
    {
        var driver = GeneratorDriver(f, []).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }

    [Fact]
    public Task TestLibraryWrap()
    {
        const TargetFramework f = TargetFramework.Net80;
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropLibraryWrap", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDllWrap(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropDllWrap", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDifferentName(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropDifferentName", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestScalarParameters(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropScalarParameters", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestRefParameters(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropRefParameters", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestInParameters(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropInParameters", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestOutParameters(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropOutParameters", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestNullableParameters(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropNullableParameters", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestNullableReturn(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropNullableReturn", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestScalarReturn(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropScalarReturn", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWrapperReturn(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWrapperReturn", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestErrorMethod(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropErrorMethod", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }

        
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasAttributes(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropHasAttributes", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnNoMatch(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnNoMatch", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnParamType(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnParamType", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnReturn(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnReturn", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }

    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnMissingImport(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnMissingImport", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnWrongNum(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnWrongNum", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnNotPartial(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropWarnNotPartial", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestErrorNoParent(TargetFramework f)
    {
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandle(f), Helpers.TestWrapper(f), Helpers.GetResource("InteropErrorNoParent", f)
        ]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault().Filter(Helpers.ExcludedResults);

        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }


    static (GeneratorDriver, Compilation) GeneratorDriver(TargetFramework f, ImmutableArray<SyntaxTree> trees, bool generateAll = true)
    {
        var frameworkReferences = f switch
        {

            TargetFramework.Net80 => Net80.References.All,
            TargetFramework.NetStandard20 => NetStandard20.References.All,
            _ => throw new ArgumentOutOfRangeException(nameof(f), f, null)
        };

        PortableExecutableReference markerReference = MetadataReference.CreateFromFile(typeof(CbsgGenerateWrapperAttribute).Assembly.Location);

        var compilation = CSharpCompilation.Create(Helpers.AssemblyName, syntaxTrees: [..trees,], references:
            [
                markerReference, ..frameworkReferences
            ],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new InteropGenerator(generateAll);

        var driver = CSharpGeneratorDriver.Create([generator.AsSourceGenerator()], parseOptions: new CSharpParseOptions(preprocessorSymbols: f.ToPreprocessor()));
        return (driver.RunGenerators(compilation), compilation);
    }
}