using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using MMKiwi.CBindingSG;
using MMKiwi.CBindingSG.SourceGenerator;
using MMKiwi.CBindingSG.SourceGeneratorTests;

using Xunit.Abstractions;

namespace SourceGeneratorTests;

[UsesVerify]
public class HandleGeneratorTest
{

    private readonly ITestOutputHelper output;

    public HandleGeneratorTest(ITestOutputHelper output)
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
    public Task TestErrorNotPartial()
    {
        SyntaxTree source = Helpers.GetResource("HandleErrorNotPartial");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();

        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestWarnNotSealedOrAbstract()
    {
        SyntaxTree source = Helpers.GetResource("HandleNotSealedOrAbstract");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestWarnNoBase()
    {
        SyntaxTree source = Helpers.GetResource("HandleNoBase");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestHasConstructor()
    {
        SyntaxTree source = Helpers.GetResource("HandleHasConstructor");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }
    
    [Fact]
    public Task TestDoNotGenerateDoesntOwn()
    {
        SyntaxTree source = Helpers.GetResource("HandleDoNotGenerateDoesntOwn");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }
    
    [Fact]
    public Task TestDoNotGenerateOwns()
    {
        SyntaxTree source = Helpers.GetResource("HandleDoNotGenerateOwns");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }
    
    [Fact]
    public Task TestHasOwns()
    {
        SyntaxTree source = Helpers.GetResource("HandleHasOwns");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }

    [Fact]
    public Task TestHasDoesntOwn()
    {
        SyntaxTree source = Helpers.GetResource("HandleHasDoesntOwn");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }
    
    [Fact]
    public Task TestHasPrimaryConstructor()
    {
        SyntaxTree source = Helpers.GetResource("HandleHasPrimaryConstructor");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
        ]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots");
    }
    
    [Fact]
    public Task TestHasExtraneousConstructor()
    {
        SyntaxTree source = Helpers.GetResource("HandleHasExtraneousConstructor");

        var driver = GeneratorDriver([
            Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, source
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
                  public const MemberVisibility ConstructorVisibility = MemberVisibility.{{memberVisibility.ToString()}};
              }
              """);

        SyntaxTree source = Helpers.GetResource("HandleConstructorVisibility");
        var driver = GeneratorDriver([Helpers.Global, Helpers.TestError, Helpers.TestHandleBase, constantSource, source]).OutputDiagnostics(output);

        var runResult = driver.GetRunResult().Results.Single();
        return Verify(runResult).UseDirectory("snapshots").UseParameters(memberVisibility);
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
        var generator = new HandleGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);
        return (driver.RunGenerators(compilation), compilation);
    }
}