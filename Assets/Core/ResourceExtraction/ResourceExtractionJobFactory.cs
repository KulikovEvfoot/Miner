using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public class ResourceExtractionJobFactory
    {
        private readonly IEnumerable<ITransition> m_Transitions;
        private readonly RewardCollectorsController m_RewardCollectorsService;

        public ResourceExtractionJobFactory(
            IEnumerable<ITransition> transitions,
            RewardCollectorsController rewardCollectorsService)
        {
            m_Transitions = transitions;
            m_RewardCollectorsService = rewardCollectorsService;
        }

        public ResourceExtractionJob Create()
        {
            var job = new ResourceExtractionJob(m_Transitions, m_RewardCollectorsService);
            return job;
        }
    }
}