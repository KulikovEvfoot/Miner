using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;

namespace Core.ResourceExtraction.Executor.Deliverer
{
    public class ResourceExtractionDeliverer : IJobOperationExecutor
    {
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly List<IReward> m_CollectedRewards;

        public ResourceExtractionDeliverer(
            RewardCollectorsController rewardCollectorsService,
            List<IReward> collectedRewards)
        {
            m_RewardCollectorsService = rewardCollectorsService;
            m_CollectedRewards = collectedRewards;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            if (jobOperationInfo is ResourceExtractionDelivererInfo info)
            {
                m_RewardCollectorsService.CollectRewards(m_CollectedRewards);
                m_CollectedRewards.Clear();
            }
        }
    }
}