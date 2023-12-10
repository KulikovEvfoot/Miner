using Services.Job.Runtime;

namespace Core.ResourceExtraction.Executor.Starter
{
    public class ResourceExtractionStarterInfo : IJobOperationInfo
    {
        public IResourceExtractor ResourceExtractor;
        public float Time; //можно подставить время после перезапуска игры

        public ResourceExtractionStarterInfo(IResourceExtractor resourceExtractor, float time)
        {
            ResourceExtractor = resourceExtractor;
            Time = time;
        }
    }
}