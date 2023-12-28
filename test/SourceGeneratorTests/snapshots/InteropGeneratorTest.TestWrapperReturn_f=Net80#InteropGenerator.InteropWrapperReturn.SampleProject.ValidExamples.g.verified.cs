//HintName: InteropGenerator.InteropWrapperReturn.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropWrapperReturn
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial FullyGenerated.TestWrapper TestMethod()
    {
        FullyGenerated.TestWrapper __return_value = null!;
        FullyGenerated.TestHandle.Owns __return_value_raw;
         __return_value_raw = _TestMethod();
        __return_value = MMKiwi.CBindingSG.CbsgConstructionHelper.Construct<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__return_value_raw);
        return __return_value;

    }
}
}
