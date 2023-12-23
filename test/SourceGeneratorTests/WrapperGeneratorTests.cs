// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;

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

    [Fact]
    public Task Driver()
    {
        var driver = GeneratorDriver([]).OutputDiagnostics(output);

        return Verify(driver);
    }

    [Fact]
    public Task RunResults()
    {
        var driver = GeneratorDriver([]).OutputDiagnostics(output);

        var runResults = driver.GetRunResult();
        return Verify(runResults);
    }

    [Fact]
    public Task TestDefaultWrapper()
    {
        SyntaxTree source = Helpers.GetResource("WrapperDefault");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestDefaultWrapperNeverOwns()
    {
        SyntaxTree source = Helpers.GetResource("WrapperDefaultNeverOwns");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestHasConstructor()
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasConstructor");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestHasImplicitHandle()
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasImplicitHandle");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestHasExplicitHandle()
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasExplicitHandle");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestWarnDoesNotImplement()
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnDoesNotImplement");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestWarnNoPartial()
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnNoPartial");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestWarnMissingIDisposable()
    {
        SyntaxTree source = Helpers.GetResource("WrapperWarnMissingIDisposable");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestConstructorVisibility(MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperConstructorVisibility");
        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots").UseParameters(memberVisibility);
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestHandleVisibility(MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperHandleVisibility");
        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots").UseParameters(memberVisibility);
    }

    [Theory]
    [ClassData(typeof(MemberVisibilities))]
    public Task TestHandleSetVisibility(MemberVisibility memberVisibility)
    {
        SyntaxTree constantSource = CSharpSyntaxTree.ParseText(
            $$"""
              public static class StaticValue
              {
                  public const MemberVisibility Visibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("WrapperHandleSetVisibility");
        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, constantSource, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots").UseParameters(memberVisibility);
    }

    [Fact]
    public Task TestHasEverything()
    {
        SyntaxTree source = Helpers.GetResource("WrapperHasEverything");
        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, Helpers.SampleHandles, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    static (GeneratorDriver, Compilation) GeneratorDriver(ImmutableArray<SyntaxTree> trees)
    {
        string dotNetAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;

        IEnumerable<PortableExecutableReference> references =
        [

            MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "mscorlib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.dll")),
            MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Core.dll")),
            MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Private.CoreLib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(typeof(IConstructableWrapper<,>).Assembly.Location)
        ];

        var compilation = CSharpCompilation.Create(Helpers.AssemblyName, syntaxTrees: trees, references: references)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        ;
        var generator = new WrapperGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);
        return (driver.RunGenerators(compilation), compilation);
    }
}