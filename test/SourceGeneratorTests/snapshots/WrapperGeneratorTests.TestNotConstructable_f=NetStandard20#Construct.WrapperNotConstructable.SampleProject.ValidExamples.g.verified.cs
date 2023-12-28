//HintName: Construct.WrapperNotConstructable.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperNotConstructable
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    private WrapperNotConstructable(global::SampleProject.SampleHandleNeverOwns handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    internal global::SampleProject.SampleHandleNeverOwns Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    global::SampleProject.SampleHandleNeverOwns IHasHandle<global::SampleProject.SampleHandleNeverOwns>.Handle => Handle;
}
}
