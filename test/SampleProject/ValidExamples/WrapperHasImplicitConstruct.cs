// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

[CbsgGenerateWrapper]
public sealed partial class WrapperHasImplicitConstruct : IConstructableWrapper<WrapperHasImplicitConstruct, SampleHandleNeverOwns>, IHasHandle<SampleHandleNeverOwns>
{
    private static WrapperHasImplicitConstruct Construct(SampleHandleNeverOwns handle) => throw new NotImplementedException();
}