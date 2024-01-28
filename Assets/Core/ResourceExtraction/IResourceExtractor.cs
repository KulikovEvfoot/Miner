using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public interface IResourceExtractor
    {
        void StartJob(IResourceExtractionJob job);
        void ResourceGathering(IEnumerable<ITransition> transitions);
        void FinalizeJob();
    }
}