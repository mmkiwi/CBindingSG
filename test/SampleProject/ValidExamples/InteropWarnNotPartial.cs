// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class InteropWarnNotPartial
{
#if NET7_0_OR_GREATER 
    [LibraryImport("TEST_DLL", StringMarshalling = StringMarshalling.Utf16)]
    private static partial void TestMethod(int a, string b, FullyGenerated.TestHandle c);
#else
    [DllImport("TEST_DLL")]
    private static extern void TestMethod(int a, string b, FullyGenerated.TestHandle c);
#endif

    [CbsgWrapperMethod]
    public static void TestMethod(int a, string b, FullyGenerated.TestWrapper c) { }
}