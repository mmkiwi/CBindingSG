// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject;

public static partial class SampleInterop
{
#if NET7_0_OR_GREATER
    [LibraryImport("DLL_NAME")]
    private static partial int SampleFunc(SampleHandleNeverOwns handleNeverOwns);
#else
    [DllImport("DLL_NAME")]
    private static extern int SampleFunc(SampleHandleNeverOwns handleNeverOwns);
#endif

    // This method will use the error method defined on the assmebly
    [CbsgWrapperMethod]
    public static partial int SampleFunc(SampleWrapper testWrapper);
}