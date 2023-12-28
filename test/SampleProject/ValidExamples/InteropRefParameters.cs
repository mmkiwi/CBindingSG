// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class InteropRefParameters
{
#if NET7_0_OR_GREATER 
    [LibraryImport("TEST_DLL", StringMarshalling = StringMarshalling.Utf16)]
    private static partial void TestMethod(ref int a, ref FullyGenerated.TestHandle.Owns handle);
#else
    [DllImport("TEST_DLL")]
    private static extern void TestMethod(ref int a, ref FullyGenerated.TestHandle.Owns handle);
#endif

    [CbsgWrapperMethod()]
    public static partial void TestMethod(ref int a, ref FullyGenerated.TestWrapper handle);
}