//HintName: InteropGenerator.InteropNullableParameters.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropNullableParameters
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethod(FullyGenerated.TestWrapper? handle)
    {
        FullyGenerated.TestHandle.Owns __param_handle = (handle as IHasHandle<FullyGenerated.TestHandle.Owns>)?.Handle ?? MMKiwi.CBindingSG.CbsgConstructionHelper.GetNullHandle<FullyGenerated.TestHandle.Owns>();
         TestMethod(__param_handle);

    }
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethodIn(in FullyGenerated.TestWrapper? handle)
    {
        FullyGenerated.TestHandle.Owns __param_handle = (handle as IHasHandle<FullyGenerated.TestHandle.Owns>)?.Handle ?? MMKiwi.CBindingSG.CbsgConstructionHelper.GetNullHandle<FullyGenerated.TestHandle.Owns>();
         TestMethodIn(in __param_handle);

    }
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethodOut(out FullyGenerated.TestWrapper? handle)
    {
        FullyGenerated.TestHandle.Owns __out_handle_raw;
         TestMethodOut(out __out_handle_raw);
        handle =  MMKiwi.CBindingSG.CbsgConstructionHelper.ConstructNullable<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__out_handle_raw);

    }
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethodRef(ref FullyGenerated.TestWrapper? handle)
    {
        FullyGenerated.TestHandle.Owns __ref_handle_raw = (handle as IHasHandle<FullyGenerated.TestHandle.Owns>)?.Handle ?? MMKiwi.CBindingSG.CbsgConstructionHelper.GetNullHandle<FullyGenerated.TestHandle.Owns>();
         TestMethodRef(ref __ref_handle_raw);
        handle =  MMKiwi.CBindingSG.CbsgConstructionHelper.ConstructNullable<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__ref_handle_raw);

    }
}
}
