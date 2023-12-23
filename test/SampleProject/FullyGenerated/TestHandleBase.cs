using Microsoft.Win32.SafeHandles;

namespace SampleProject.FullyGenerated;

public abstract class TestHandleBase(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle)
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}