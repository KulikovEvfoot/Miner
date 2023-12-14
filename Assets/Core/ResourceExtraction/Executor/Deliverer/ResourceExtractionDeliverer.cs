using System.Collections.Generic;
using Core.ResourceExtraction.Executor.Starter;
using Services.Currency.Runtime.Rewards;
using Services.Job.Runtime;

namespace Core.ResourceExtraction.Executor.Deliverer
{
    public class ResourceExtractionDeliverer : IJobOperationExecutor
    {
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly List<IReward> m_CollectedRewards;
        private readonly IJob m_Job;

        public ResourceExtractionDeliverer(RewardCollectorsController rewardCollectorsService,
            List<IReward> collectedRewards, IJob job)
        {
            m_RewardCollectorsService = rewardCollectorsService;
            m_CollectedRewards = collectedRewards;
            m_Job = job;
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