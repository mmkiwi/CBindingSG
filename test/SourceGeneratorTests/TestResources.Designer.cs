using System;
using System.Resources;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;
internal sealed class TestResources
{
    private static Lazy<ResourceManager> resourceMan = new(() => new("MMKiwi.CBindingSG.SourceGeneratorTests.TestResources", typeof(TestResources).Assembly));
    
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Resources.ResourceManager ResourceManager => resourceMan.Value;
}