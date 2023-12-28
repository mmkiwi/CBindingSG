// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

#if !CBSG_OMITHASHANDLE && !CBSG_OMITALL
#nullable enable
namespace MMKiwi.CBindingSG;

public interface IHasHandle<out THandle> where THandle : global::System.Runtime.InteropServices.SafeHandle
{
    public THandle Handle { get; }
}
#endif