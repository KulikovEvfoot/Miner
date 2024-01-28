using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;

namespace Core.ResourceExtraction.Executor.Gatherer
{
    public class ResourceExtractionGatherer : IJobOperationExecutor
    {
        private readonly List<IReward> m_CollectedRewards;

        public ResourceExtractionGatherer(List<IReward> collectedRewards)
        {
            m_CollectedRewards = collectedRewards;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            if (jobOperationInfo is ResourceExtractionGatheringInfo info)
            {
                foreach (var reward in info.Rewards)
                {
                    m_CollectedRewards.Add(reward);
                }
            }
        }
    }
}