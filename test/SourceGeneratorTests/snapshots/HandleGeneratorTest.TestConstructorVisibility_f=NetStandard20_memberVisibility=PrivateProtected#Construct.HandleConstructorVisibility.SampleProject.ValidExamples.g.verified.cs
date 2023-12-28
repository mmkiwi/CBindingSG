//HintName: Construct.HandleConstructorVisibility.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

internal abstract partial class HandleConstructorVisibility
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public static HandleConstructorVisibility Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleConstructorVisibility(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        internal static Owns Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleConstructorVisibility(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        internal static DoesntOwn Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    private protected HandleConstructorVisibility(bool ownsHandle): base(ownsHandle) { }
}
}
