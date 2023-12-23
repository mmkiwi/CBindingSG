// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

[CbsgGenerateWrapper]
public sealed partial class WrapperHasEverything : IConstructableWrapper<WrapperHasEverything, SampleHandleNeverOwns>, IHasHandle<SampleHandleNeverOwns>
{
#if NET7_0_OR_GREATER
    static WrapperHasEverything IConstructableWrapper<WrapperHasEverything, SampleHandleNeverOwns>.Construct(SampleHandleNeverOwns handle) => new(handle);
#else
    public static WrapperHasEverything Construct(SampleHandleNeverOwns handle) => new(handle);
#endif

    public WrapperHasEverything(SampleHandleNeverOwns handle) => Handle = handle;

    public SampleHandleNeverOwns Handle { get; }
}