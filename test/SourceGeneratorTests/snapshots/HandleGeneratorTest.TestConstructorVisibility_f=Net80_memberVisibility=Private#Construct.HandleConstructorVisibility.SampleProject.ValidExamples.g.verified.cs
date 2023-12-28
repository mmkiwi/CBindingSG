//HintName: Construct.HandleConstructorVisibility.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

internal abstract partial class HandleConstructorVisibility
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    static HandleConstructorVisibility IConstructableHandle<HandleConstructorVisibility>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public class Owns() : HandleConstructorVisibility(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        static Owns MMKiwi.CBindingSG.IConstructableHandle<Owns>.Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public class DoesntOwn() : HandleConstructorVisibility(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        static DoesntOwn MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>.Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    private HandleConstructorVisibility(bool ownsHandle): base(ownsHandle) { }
}
}
