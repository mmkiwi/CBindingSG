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
public partial class SampleHandleNeverOwns() : SampleHandle(false)
{
}