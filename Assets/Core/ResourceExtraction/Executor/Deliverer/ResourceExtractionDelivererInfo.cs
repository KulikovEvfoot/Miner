using Services.Job.Runtime;

namespace Core.ResourceExtraction.Executor.Deliverer
{
    public class ResourceExtractionDelivererInfo : IJobOperationInfo
    {
        public IResourceExtractor ResourceExtractor;
        public float Time;

        public ResourceExtractionDelivererInfo(IResourceExtractor resourceExtractor, float time)
        {
            ResourceExtractor = resourceExtractor;
            Time = time;
        }
    }
}