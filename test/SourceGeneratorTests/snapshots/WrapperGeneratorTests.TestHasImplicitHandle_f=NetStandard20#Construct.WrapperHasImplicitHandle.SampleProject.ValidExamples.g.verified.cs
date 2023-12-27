//HintName: Construct.WrapperHasImplicitHandle.SampleProject.ValidExamples.g.cs
using SampleProject;
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperHasImplicitHandle
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    static global::SampleProject.ValidExamples.WrapperHasImplicitHandle Construct(global::SampleProject.SampleHandleNeverOwns handle) => new(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    private WrapperHasImplicitHandle(global::SampleProject.SampleHandleNeverOwns handle) => Handle = handle;
}
}
