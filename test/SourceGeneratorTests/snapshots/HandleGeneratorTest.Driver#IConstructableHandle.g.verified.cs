//HintName: IConstructableHandle.g.cs
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

#if !CBSG_OMITINTERNAL
#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace MMKiwi.CBindingSG;

#if !NET7_0_OR_GREATER
[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Type arguments needed for static methods")]
#endif
internal interface IConstructableHandle<out THandle> where THandle : SafeHandle
{
#if NET7_0_OR_GREATER
    static abstract THandle Construct(bool ownsHandle);
#endif
}
#endif