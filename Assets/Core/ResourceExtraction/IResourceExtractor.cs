using System.Collections.Generic;
using Services.Job.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public interface IResourceExtractor : IEmployee
    {
        void ResourceGathering(IEnumerable<ITransition> transitions);
        void FinalizeJob();
    }
}