//HintName: Construct.HandleConstructorVisibility.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

internal abstract partial class HandleConstructorVisibility
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static HandleConstructorVisibility Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleConstructorVisibility(true);
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleConstructorVisibility(false);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    protected HandleConstructorVisibility(bool ownsHandle): base(ownsHandle) { }
}
}
