// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class HandleLibraryWrapIn
{
    [LibraryImport("TEST_DLL")]
    public static partial void TestMethod(FullyGenerated.TestHandle handle);

    [CbsgWrapperMethod]
    public static partial void TestMethod(FullyGenerated.TestWrapper wrapper);
}
