// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class InteropNullableParameters
{
#if NET7_0_OR_GREATER 
    [LibraryImport("TEST_DLL")]
    private static partial void TestMethod(FullyGenerated.TestHandle.Owns handle);
    [LibraryImport("TEST_DLL")]
    private static partial void TestMethodIn(in FullyGenerated.TestHandle.Owns handle);
    [LibraryImport("TEST_DLL")]
    private static partial void TestMethodOut(out FullyGenerated.TestHandle.Owns handle);
    [LibraryImport("TEST_DLL")]
    private static partial void TestMethodRef(ref FullyGenerated.TestHandle.Owns handle);
#else
    [DllImport("TEST_DLL")]
    private static extern void TestMethod(FullyGenerated.TestHandle.Owns handle);
    [DllImport("TEST_DLL")]
    private static extern void TestMethodIn(in FullyGenerated.TestHandle.Owns handle);
    [DllImport("TEST_DLL")]
    private static extern void TestMethodOut(out FullyGenerated.TestHandle.Owns handle);
    [DllImport("TEST_DLL")]
    private static extern void TestMethodRef(ref FullyGenerated.TestHandle.Owns handle);
#endif

    [CbsgWrapperMethod()]
    public static partial void TestMethod(FullyGenerated.TestWrapper? handle);
    [CbsgWrapperMethod()]
    public static partial void TestMethodIn(in FullyGenerated.TestWrapper? handle);
    [CbsgWrapperMethod()]
    public static partial void TestMethodOut(out FullyGenerated.TestWrapper? handle);
    [CbsgWrapperMethod()]
    public static partial void TestMethodRef(ref FullyGenerated.TestWrapper? handle);
    
}