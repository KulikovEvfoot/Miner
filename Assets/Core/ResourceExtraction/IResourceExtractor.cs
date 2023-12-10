using System.Collections.Generic;
using Services.Job.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public interface IResourceExtractor : IEmployee, IRouteMoveListener
    {
        void ResourceGathering(IEnumerable<ITransition> transitions, float time);
        void FinalizeJob();
    }
}