﻿//HintName: Construct.WrapperConstructorVisibility.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public sealed partial class WrapperConstructorVisibility
{ 

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static global::SampleProject.ValidExamples.WrapperConstructorVisibility global::MMKiwi.CBindingSG.IConstructableWrapper<global::SampleProject.ValidExamples.WrapperConstructorVisibility, global::SampleProject.SampleHandleNeverOwns>.Construct(global::SampleProject.SampleHandleNeverOwns handle) => new(handle);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    internal global::SampleProject.SampleHandleNeverOwns Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    global::SampleProject.SampleHandleNeverOwns IHasHandle<global::SampleProject.SampleHandleNeverOwns>.Handle => Handle;
}
}
