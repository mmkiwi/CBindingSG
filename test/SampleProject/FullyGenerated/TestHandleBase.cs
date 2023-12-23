// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Microsoft.Win32.SafeHandles;

namespace SampleProject.FullyGenerated;

public abstract class TestHandleBase(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle)
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}