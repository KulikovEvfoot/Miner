using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public class ResourceExtractionJobFactory
    {
        private readonly IRoute m_Route;
        private readonly RewardCollectorsController m_RewardCollectorsService;

        public ResourceExtractionJobFactory(
            IRoute route,
            RewardCollectorsController rewardCollectorsService)
        {
            m_Route = route;
            m_RewardCollectorsService = rewardCollectorsService;
        }

        public ResourceExtractionJob Create()
        {
            var job = new ResourceExtractionJob(m_Route, m_RewardCollectorsService);
            return job;
        }
    }
}