//HintName: Construct.HandleHasExtraneousConstructor.HandleHasExtraneousConstructorBase.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleHasExtraneousConstructorBase
{

public abstract partial class HandleHasExtraneousConstructor
{
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleHasExtraneousConstructor(true);
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleHasExtraneousConstructor(false);
}
}
}