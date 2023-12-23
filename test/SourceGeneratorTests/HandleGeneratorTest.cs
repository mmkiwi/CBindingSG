using System.Collections.Immutable;
using System.Reflection.Metadata;

using Basic.Reference.Assemblies;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using MMKiwi.CBindingSG.SourceGenerator;

using Xunit.Abstractions;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

[UsesVerify]
public class HandleGeneratorTest
{

    private readonly ITestOutputHelper output;

    public HandleGeneratorTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task Driver(TargetFramework f)
    {
        var driver = GeneratorDriver([], f).OutputDiagnostics(output);

        return Verify(driver).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task RunResults(TargetFramework f)
    {
        var driver = GeneratorDriver([], f).OutputDiagnostics(output);

        var runResults = driver.GetRunResult();
        return Verify(runResults).UseDirectory($"snapshots").UseParameters(f);
    }


    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestErrorNotPartial(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleErrorNotPartial", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();

        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnNotSealedOrAbstract(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleNotSealedOrAbstract", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestWarnNoBase(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleNoBase", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasConstructor(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleHasConstructor", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDoNotGenerateDoesntOwn(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleDoNotGenerateDoesntOwn", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestDoNotGenerateOwns(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleDoNotGenerateOwns", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasOwns(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleHasOwns", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasDoesntOwn(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleHasDoesntOwn", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasPrimaryConstructor(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleHasPrimaryConstructor", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f);
    }

    [Theory]
    [ClassData(typeof(TargetFrameworks))]
    public Task TestHasExtraneousConstructor(TargetFramework f)
    {
        SyntaxTree source = Helpers.GetResource("HandleHasExtraneousConstructor", f);

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), source
        ], f).OutputDiagnostics(output);

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

        SyntaxTree source = Helpers.GetResource("HandleConstructorVisibility", f);
        var driver = GeneratorDriver([Helpers.Global, Helpers.TestError, Helpers.TestHandleBase(f), constantSource, source], f).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory($"snapshots").UseParameters(f, memberVisibility);
    }

    static (GeneratorDriver, Compilation) GeneratorDriver(ImmutableArray<SyntaxTree> trees, TargetFramework f)
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

        var generator = new HandleGenerator();

        var driver = CSharpGeneratorDriver.Create([generator.AsSourceGenerator()], parseOptions: new CSharpParseOptions(preprocessorSymbols: f.ToPreprocessor()));
        return (driver.RunGenerators(compilation), compilation);
    }
}