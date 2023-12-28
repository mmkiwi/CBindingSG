//HintName: Construct.HandleNotSealedOrAbstract.SampleProject.ValidExamples.g.cs
using SampleProject.FullyGenerated;
#nullable enable
namespace SampleProject.ValidExamples {

internal partial class HandleNotSealedOrAbstract
{

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    static HandleNotSealedOrAbstract IConstructableHandle<HandleNotSealedOrAbstract>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class Owns() : HandleNotSealedOrAbstract(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
        static Owns MMKiwi.CBindingSG.IConstructableHandle<Owns>.Construct(bool ownsHandle)
        {
           if(!ownsHandle)
               throw new InvalidOperationException("Cannot construct Owns that does not own handle");
           return new();
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleNotSealedOrAbstract(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        static DoesntOwn MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>.Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    protected HandleNotSealedOrAbstract(bool ownsHandle): base(ownsHandle) { }
}
}
