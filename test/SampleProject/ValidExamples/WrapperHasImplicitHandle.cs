// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using SampleProject;

namespace SampleProject.ValidExamples;

[CbsgGenerateWrapper]
public sealed partial class WrapperHasImplicitHandle : IConstructableWrapper<WrapperHasImplicitHandle, SampleHandleNeverOwns>, IHasHandle<SampleHandleNeverOwns>
{
    public SampleHandleNeverOwns Handle { get; set; }
}
