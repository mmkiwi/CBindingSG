//HintName: Construct.WrapperHasExplicitHandle.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperHasExplicitHandle
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static global::SampleProject.ValidExamples.WrapperHasExplicitHandle Construct(global::SampleProject.SampleHandleNeverOwns handle) => new(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    private WrapperHasExplicitHandle(global::SampleProject.SampleHandleNeverOwns handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    internal global::SampleProject.SampleHandleNeverOwns Handle { get; }
}
}
