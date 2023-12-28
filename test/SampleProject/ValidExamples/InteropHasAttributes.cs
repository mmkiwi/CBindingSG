// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel;

namespace SampleProject.ValidExamples;

public partial class InteropHasAttributes
{
#if NET7_0_OR_GREATER
    [LibraryImport("TEST_DLL", StringMarshalling = StringMarshalling.Utf16)]
    private static partial void TestMethod(FullyGenerated.TestHandle c, int b);
    
#else
    [DllImport("TEST_DLL")]
    private static extern void TestMethod( FullyGenerated.TestHandle c, int b);
#endif
    
    [CbsgWrapperMethod]
    public static partial void TestMethod([Description("test attribute")] FullyGenerated.TestWrapper c, 
        [Description("B"), Bindable(true)]
        [Optional]
        int b);
}