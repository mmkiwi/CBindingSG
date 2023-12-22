// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;

namespace MMKiwi.CBindingSG;

public static class CbsgConstructionHelper
{
#if NET7_0_OR_GREATER
    public static TRes Construct<TRes, THandle>(THandle handle)
        where TRes : class, IConstructableWrapper<TRes, THandle>
        where THandle : SafeHandle
    {
        return handle.IsInvalid
            ? throw new InvalidOperationException("Cannot marshal null handle")
            : TRes.Construct(handle);
    }

    public static TRes? ConstructNullable<TRes, THandle>(THandle handle)
        where TRes : class, IConstructableWrapper<TRes, THandle>
        where THandle : SafeHandle
    {
        return handle.IsInvalid ? null : TRes.Construct(handle);
    }

    public static THandle GetNullHandle<THandle>()
        where THandle : SafeHandle, IConstructableHandle<THandle>
    {
        return THandle.Construct(false);
    }
#else
    public static TRes Construct<TRes, THandle>(THandle handle)
        where TRes : class, IConstructableWrapper<TRes, THandle>
        where THandle : SafeHandle
    {
        var constructMethod = FindWrapperConstructMethod<TRes, THandle>();
        
        return handle.IsInvalid
            ? throw new InvalidOperationException("Cannot marshal null handle")
            : (TRes)constructMethod.Invoke(null, [handle]);
    }

    public static TRes? ConstructNullable<TRes, THandle>(THandle handle)
        where TRes : class, IConstructableWrapper<TRes, THandle>
        where THandle : SafeHandle
    {
        var constructMethod = FindWrapperConstructMethod<TRes, THandle>();

        return handle.IsInvalid ? null : (TRes)constructMethod.Invoke(null, [handle]);
    }
    public static THandle GetNullHandle<THandle>()
        where THandle : SafeHandle, IConstructableHandle<THandle>
    {
        return (THandle)FindHandleConstructMethod<THandle>().Invoke(null, [false]);
    }
    
    private static MethodInfo FindHandleConstructMethod<THandle>()
        where THandle : SafeHandle, IConstructableHandle<THandle>
    {
        // .NET before 7 can't use static abstract interface members so we have to use reflection.
        var constructMethod = typeof(THandle).GetMethod("Construct", BindingFlags.Static | BindingFlags.Public, null, [typeof(bool)], null);
        if (constructMethod is null || constructMethod.ReturnType != typeof(THandle))
            throw new InvalidOperationException($"Type ${typeof(THandle).Name} does not contain a static method Construct that takes a bool " +
                                                $"and returns a ${typeof(THandle).Name}");
        return constructMethod;
    }
    
    private static MethodInfo FindWrapperConstructMethod<TRes, THandle>()
        where TRes : class, IConstructableWrapper<TRes, THandle> 
        where THandle : SafeHandle
    {
        // .NET before 7 can't use static abstract interface members so we have to use reflection.
        var constructMethod = typeof(TRes).GetMethod("Construct", BindingFlags.Static | BindingFlags.Public, null, [typeof(THandle)], null);
        if (constructMethod is null || constructMethod.ReturnType != typeof(TRes))
            throw new InvalidOperationException($"Type ${typeof(TRes).Name} does not contain a static method Construct that takes a {typeof(THandle).Name} " +
                                                $"and returns a ${typeof(TRes).Name}");
        return constructMethod;
    }


#endif
    
}
