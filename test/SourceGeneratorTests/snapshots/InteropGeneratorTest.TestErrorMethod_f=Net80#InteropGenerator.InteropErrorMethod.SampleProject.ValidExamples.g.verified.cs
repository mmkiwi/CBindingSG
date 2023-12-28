//HintName: InteropGenerator.InteropErrorMethod.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropErrorMethod
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static partial FullyGenerated.TestWrapper TestMethod()
    {
        FullyGenerated.TestWrapper __return_value = null!;
        FullyGenerated.TestHandle.Owns __return_value_raw;
         __return_value_raw = _TestMethod();
        global::SampleProject.ValidExamples.InteropErrorMethod.Error.ThrowIfError();
        __return_value = MMKiwi.CBindingSG.CbsgConstructionHelper.Construct<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__return_value_raw);
        return __return_value;

    }
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static partial FullyGenerated.TestWrapper TestMethod2()
    {
        FullyGenerated.TestWrapper __return_value = null!;
        FullyGenerated.TestHandle.Owns __return_value_raw;
         __return_value_raw = _TestMethod();
        global::SampleProject.ValidExamples.InteropErrorMethod.Error.ThrowIfError2();
        __return_value = MMKiwi.CBindingSG.CbsgConstructionHelper.Construct<FullyGenerated.TestWrapper, FullyGenerated.TestHandle.Owns>(__return_value_raw);
        return __return_value;

    }
}
}
