//HintName: Construct.HandleDoNotGenerateOwns.SampleProject.ValidExamples.g.cs
#nullable enable
namespace SampleProject.ValidExamples {

public abstract partial class HandleDoNotGenerateOwns
{
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    public class DoesntOwn() : HandleDoNotGenerateOwns(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
        static DoesntOwn MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>.Construct(bool ownsHandle)
        {
           if(ownsHandle)
               throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
           return new();
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    protected HandleDoNotGenerateOwns(bool ownsHandle): base(ownsHandle) { }
}
}
