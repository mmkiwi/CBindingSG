//HintName: Construct.HandleHasOwns.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleHasOwns
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static HandleHasOwns Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleHasOwns(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        internal static DoesntOwn Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    protected HandleHasOwns(bool ownsHandle): base(ownsHandle) { }
}
}
