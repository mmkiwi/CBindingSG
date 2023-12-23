// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

[CbsgGenerateWrapper(HandleSetVisibility = StaticValue.Visibility)]
public sealed partial class WrapperHandleSetVisibility : IConstructableWrapper<WrapperHandleSetVisibility, SampleHandleNeverOwns>, IHasHandle<SampleHandleNeverOwns>
{
}