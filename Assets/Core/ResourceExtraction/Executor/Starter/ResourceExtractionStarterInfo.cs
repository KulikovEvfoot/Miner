namespace Core.ResourceExtraction.Executor.Starter
{
    public class ResourceExtractionStarterInfo : IJobOperationInfo
    {
        public IResourceExtractor ResourceExtractor;

        public ResourceExtractionStarterInfo(IResourceExtractor resourceExtractor)
        {
            ResourceExtractor = resourceExtractor;
        }
    }
}