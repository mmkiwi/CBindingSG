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
public class WrapperGeneratorTests
{
    private readonly ITestOutputHelper output;

    public WrapperGeneratorTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task Driver(TargetFramework f)
    {
        var driver = GeneratorDriver(f, []).OutputDiagnostics(output);

        return Verify(driver).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task RunResults(TargetFramework f)
    {
        var driver = GeneratorDriver(f, []).OutputDiagnostics(output);

        var runResults = driver.GetRunResult().Results.SingleOrDefault();
        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDefaultWrapper(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperDefault", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDefaultWrapperNeverOwns(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperDefaultNeverOwns", f);

        var driver = GeneratorDriver(f,[
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        GeneratorRunResult runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasConstructor(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasConstructor", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasImplicitHandle(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasImplicitHandle", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }
    
    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasImplicitConstruct(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasImplicitConstruct", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }


    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasExplicitHandle(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasExplicitHandle", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnDoesNotImplement(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnDoesNotImplement", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnNoPartial(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnNoPartial", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnMissingIDisposable(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnMissingIDisposable", f);

        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestConstructorVisibility(TargetFramework f, MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperConstructorVisibility", f);
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f, memberVisibility);
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestHandleVisibility(TargetFramework f, MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperHandleVisibility", f);
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f, memberVisibility);
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestHandleSetVisibility(TargetFramework f, MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperHandleSetVisibility", f);
        var driver = GeneratorDriver(f, [
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f, memberVisibility);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasEverything(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasEverything", f);
        var driver = GeneratorDriver(f,[
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    static (GeneratorDriver, Compilation) GeneratorDriver(TargetFramework f, ImmutableArray<SyntaxTree> trees)
    {
        var frameworkReferences = f switch
        {

            TargetFramework.Net80 => Net80.References.All,
            TargetFramework.NetStandard20 => NetStandard20.References.All,
            _ => throw new ArgumentOutOfRangeException(nameof(f), f, null)
        };
        
        PortableExecutableReference markerReference = MetadataReference.CreateFromFile(typeof(CbsgGenerateWrapperAttribute).Assembly.Location);
        
        var compilation = CSharpCompilation.Create(Helpers.AssemblyName, syntaxTrees: [..trees,] , references: 
                [
                    markerReference, ..frameworkReferences
                ], 
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        var generator = new WrapperGenerator();

        var driver = CSharpGeneratorDriver.Create([generator.AsSourceGenerator()], parseOptions: new CSharpParseOptions(preprocessorSymbols: f.ToPreprocessor()));
        return (driver.RunGenerators(compilation), compilation);
    }
}