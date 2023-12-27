// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class InteropErrorMethod
{
#if NET7_0_OR_GREATER 
    [LibraryImport("TEST_DLL", StringMarshalling = StringMarshalling.Utf16)]
    private static partial FullyGenerated.TestHandle.Owns _TestMethod();
#else
    [DllImport("TEST_DLL")]
    private static extern FullyGenerated.TestHandle.Owns _TestMethod();
#endif

    [CbsgWrapperMethod(MethodName = nameof(_TestMethod))]
    public static partial FullyGenerated.TestWrapper TestMethod();
}