//HintName: InteropGenerator.InteropHasAttributes.SampleProject.ValidExamples.g.cs
using System.ComponentModel;
#nullable enable
namespace SampleProject.ValidExamples {
public partial class InteropHasAttributes
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
    public static partial void TestMethod(FullyGenerated.TestWrapper c, 
        int b)
    {
        if(c is null)
          throw new ArgumentNullException("c");
        FullyGenerated.TestHandle __param_c = ((IHasHandle<FullyGenerated.TestHandle>)c).Handle;
         TestMethod(__param_c, b);

    }
}
}
