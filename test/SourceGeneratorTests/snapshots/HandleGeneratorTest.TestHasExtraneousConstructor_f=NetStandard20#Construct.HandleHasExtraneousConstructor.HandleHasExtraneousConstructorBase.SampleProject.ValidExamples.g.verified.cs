//HintName: Construct.HandleHasExtraneousConstructor.HandleHasExtraneousConstructorBase.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleHasExtraneousConstructorBase
{

public abstract partial class HandleHasExtraneousConstructor
{
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleHasExtraneousConstructor(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        internal static Owns Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleHasExtraneousConstructor(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        internal static DoesntOwn Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }
}
}
}
