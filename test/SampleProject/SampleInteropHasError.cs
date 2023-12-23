// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject;

[CbsgErrorMethod(typeof(SampleError), nameof(SampleError.ThrowIfErrorMethod))]
public static partial class SampleInteropHasError
{
#if NET7_0_OR_GREATER
    [LibraryImport("DLL_NAME")]
    private static partial int SampleFunc(SampleHandle handle);
#else
    [DllImport("DLL_NAME")]
    private static extern int SampleFunc(SampleHandle handle);
#endif

    // This method will use its own error method
    [CbsgWrapperMethod]
    [CbsgErrorMethod(typeof(SampleError), nameof(SampleError.ThrowIfErrorMethod))]
    public static partial int SampleFunc(SampleWrapper testWrapper);

    // This method will use the error method defined on the class
    [CbsgWrapperMethod(MethodName = nameof(SampleFunc))]
    public static partial int SampleFunc2(SampleWrapper testWrapper);

}