namespace SampleProject;

[CbsgGenerateHandle]
public class HandleErrorNotPartial : FullyGenerated.TestHandleBase
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}