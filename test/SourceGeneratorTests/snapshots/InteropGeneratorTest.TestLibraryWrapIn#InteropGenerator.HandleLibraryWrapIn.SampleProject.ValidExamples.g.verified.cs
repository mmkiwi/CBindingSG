//HintName: InteropGenerator.HandleLibraryWrapIn.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class HandleLibraryWrapIn
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static partial void TestMethod(FullyGenerated.TestWrapper wrapper)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        FullyGenerated.TestHandle __param_wrapper = ((IHasHandle<FullyGenerated.TestHandle>)wrapper).Handle;
         TestMethod(__param_wrapper);

    }
}
}
