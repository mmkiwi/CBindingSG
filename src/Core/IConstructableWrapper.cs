// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace MMKiwi.CBindingSG;

#if !NET7_0_OR_GREATER
[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Type arguments needed for static methods")]
#endif
public interface IConstructableWrapper<out TRes, in THandle> where THandle : SafeHandle
{
#if NET7_0_OR_GREATER
    public static abstract TRes Construct(THandle handle);
#endif
}
