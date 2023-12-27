using Microsoft.Win32.SafeHandles;

namespace SampleProject.FullyGenerated;

public abstract partial class TestHandle(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle), IConstructableHandle<TestHandle>
{
#if NET7_0_OR_GREATER
    static TestHandle IConstructableHandle<TestHandle>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
#else
    public static TestHandle Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
#endif
    
    public class Owns() : TestHandle(true);
    
    public class DoesntOwn() : TestHandle(false);
    
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}