using Microsoft.Win32.SafeHandles;

namespace SampleProject;

public abstract class SampleHandle(bool ownsHandle) : SafeHandleMinusOneIsInvalid(ownsHandle)
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}

[CbsgGenerateHandle]
[CbsgNeverOwns]
public abstract partial class SampleHandleNeverOwns : SampleHandle
{
}