using Microsoft.Win32.SafeHandles;

namespace SampleProject;

public abstract class SampleHandleBase(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle)
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}