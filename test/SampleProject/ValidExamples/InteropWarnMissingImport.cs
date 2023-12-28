// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

public partial class InteropWarnMissingImport
{
    private static void TestMethod(int a, string b, FullyGenerated.TestHandle c) { }

    [CbsgWrapperMethod]
    public static partial void TestMethod(int a, string b, FullyGenerated.TestWrapper c);
}