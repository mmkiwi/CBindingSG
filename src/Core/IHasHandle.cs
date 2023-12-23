// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

#if !CBSG_OMITINTERNAL
#nullable enable
namespace MMKiwi.CBindingSG;

internal interface IHasHandle<out THandle> where THandle : SafeHandle
{
    public THandle Handle { get; }
}
#endif