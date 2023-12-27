//HintName: Construct.WrapperHasImplicitConstruct.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperHasImplicitConstruct
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    static global::SampleProject.ValidExamples.WrapperHasImplicitConstruct global::MMKiwi.CBindingSG.IConstructableWrapper<global::SampleProject.ValidExamples.WrapperHasImplicitConstruct, global::SampleProject.SampleHandleNeverOwns>.Construct(global::SampleProject.SampleHandleNeverOwns handle) => global::SampleProject.ValidExamples.WrapperHasImplicitConstruct.Construct(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    private WrapperHasImplicitConstruct(global::SampleProject.SampleHandleNeverOwns handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    internal global::SampleProject.SampleHandleNeverOwns Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    global::SampleProject.SampleHandleNeverOwns IHasHandle<global::SampleProject.SampleHandleNeverOwns>.Handle => Handle;
}
}
