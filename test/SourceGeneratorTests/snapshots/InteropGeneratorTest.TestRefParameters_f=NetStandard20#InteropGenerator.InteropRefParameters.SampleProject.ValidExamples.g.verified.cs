//HintName: InteropGenerator.InteropRefParameters.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropRefParameters
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethod(ref int a, ref FullyGenerated.TestWrapper handle)
    {
        if(handle is null)
          throw new ArgumentNullException("handle");
        FullyGenerated.TestHandle.Owns __ref_handle_raw = ((IHasHandle<FullyGenerated.TestHandle.Owns>)handle).Handle;
         TestMethod(ref a, ref __ref_handle_raw);
        handle =  MMKiwi.CBindingSG.CbsgConstructionHelper.Construct<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__ref_handle_raw);

    }
}
}
