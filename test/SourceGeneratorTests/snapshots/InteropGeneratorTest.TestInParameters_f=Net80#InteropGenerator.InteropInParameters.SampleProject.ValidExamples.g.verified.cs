//HintName: InteropGenerator.InteropInParameters.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropInParameters
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static partial void TestMethod(in int a, in FullyGenerated.TestWrapper handle)
    {
        ArgumentNullException.ThrowIfNull(handle);
        FullyGenerated.TestHandle.Owns __param_handle = ((IHasHandle<FullyGenerated.TestHandle.Owns>)handle).Handle;
         TestMethod(in a, in __param_handle);

    }
}
}
