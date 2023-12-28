//HintName: Construct.HandleHasDoesntOwn.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleHasDoesntOwn
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static HandleHasDoesntOwn IConstructableHandle<HandleHasDoesntOwn>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public class Owns() : HandleHasDoesntOwn(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        static Owns MMKiwi.CBindingSG.IConstructableHandle<Owns>.Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    protected HandleHasDoesntOwn(bool ownsHandle): base(ownsHandle) { }
}
}
