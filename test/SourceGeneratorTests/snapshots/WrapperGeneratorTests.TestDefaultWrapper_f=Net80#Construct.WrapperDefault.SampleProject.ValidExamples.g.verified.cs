//HintName: Construct.WrapperDefault.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperDefault
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static global::SampleProject.ValidExamples.WrapperDefault global::MMKiwi.CBindingSG.IConstructableWrapper<global::SampleProject.ValidExamples.WrapperDefault, global::SampleProject.SampleHandle>.Construct(global::SampleProject.SampleHandle handle) => new(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    private WrapperDefault(global::SampleProject.SampleHandle handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    internal global::SampleProject.SampleHandle Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    global::SampleProject.SampleHandle IHasHandle<global::SampleProject.SampleHandle>.Handle => Handle;
}
}
