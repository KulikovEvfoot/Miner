namespace Core.ResourceExtraction
{
    public interface IResourceExtractionJob
    {
        void Execute(IJobOperationInfo jobOperationInfo);
    }
}