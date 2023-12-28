//HintName: InteropGenerator.InteropOutParameters.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropOutParameters
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static partial void TestMethod(out int a, out FullyGenerated.TestWrapper handle)
    {
        FullyGenerated.TestHandle.Owns __out_handle_raw;
         TestMethod(out a, out __out_handle_raw);
        handle =  MMKiwi.CBindingSG.CbsgConstructionHelper.Construct<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__out_handle_raw);

    }
}
}
