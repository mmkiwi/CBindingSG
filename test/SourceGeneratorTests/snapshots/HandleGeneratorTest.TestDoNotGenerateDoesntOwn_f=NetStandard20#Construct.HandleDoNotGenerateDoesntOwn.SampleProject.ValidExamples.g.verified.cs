//HintName: Construct.HandleDoNotGenerateDoesntOwn.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleDoNotGenerateDoesntOwn
{
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleDoNotGenerateDoesntOwn(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        internal static Owns Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    protected HandleDoNotGenerateDoesntOwn(bool ownsHandle): base(ownsHandle) { }
}
}
