namespace SampleProject;

[CbsgGenerateHandle]
public class ErrorNotPartial : SafeHandleBase
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}