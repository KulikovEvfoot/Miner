using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction.Executor.Starter
{
    public class ResourceExtractionStarter : IJobOperationExecutor
    {
        private readonly IEnumerable<IPoint> m_Route;
        private readonly List<IReward> m_CollectedRewards;

        public ResourceExtractionStarter(
            IEnumerable<IPoint> route,
            List<IReward> collectedRewards)
        {
            m_Route = route;
            m_CollectedRewards = collectedRewards;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            if (jobOperationInfo is ResourceExtractionStarterInfo info)
            {
                m_CollectedRewards.Clear();
                info.ResourceExtractor.ResourceGathering(m_Route);
            }
        }
    }
}