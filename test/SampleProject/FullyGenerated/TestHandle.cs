using Microsoft.Win32.SafeHandles;

namespace SampleProject.FullyGenerated;

public abstract partial class TestHandle(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle), IConstructableHandle<TestHandle>
{
#if NET7_0_OR_GREATER
    static TestHandle IConstructableHandle<TestHandle>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
#else
    public static TestHandle Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
#endif

    public class Owns() : TestHandle(true), MMKiwi.CBindingSG.IConstructableHandle<Owns>
    {
#if NET7_0_OR_GREATER
        static Owns MMKiwi.CBindingSG.IConstructableHandle<Owns>.Construct(bool ownsHandle)
#else
        new internal static Owns Construct(bool ownsHandle)
#endif
        {
            if (ownsHandle)
                throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
            return new();
        }

    }

    public class DoesntOwn() : TestHandle(false), MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>
    {
#if NET7_0_OR_GREATER
        static DoesntOwn MMKiwi.CBindingSG.IConstructableHandle<DoesntOwn>.Construct(bool ownsHandle)
#else
        new internal static DoesntOwn Construct(bool ownsHandle)
#endif
        {
            if (ownsHandle)
                throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
            return new();
        }
    }


    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}