//HintName: Construct.WrapperDefaultNeverOwns.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperDefaultNeverOwns
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static global::SampleProject.ValidExamples.WrapperDefaultNeverOwns global::MMKiwi.CBindingSG.IConstructableWrapper<global::SampleProject.ValidExamples.WrapperDefaultNeverOwns, global::SampleProject.SampleHandleNeverOwns>.Construct(global::SampleProject.SampleHandleNeverOwns handle) => new(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    private WrapperDefaultNeverOwns(global::SampleProject.SampleHandleNeverOwns handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    internal global::SampleProject.SampleHandleNeverOwns Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    global::SampleProject.SampleHandleNeverOwns IHasHandle<global::SampleProject.SampleHandleNeverOwns>.Handle => Handle;
}
}
