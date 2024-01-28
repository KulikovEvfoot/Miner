using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using UnityEngine;
using Zenject;

namespace Core
{
    public class RewardCollectorsController
    {
        private readonly RewardCollectorsService m_RewardCollectorsService;

        [Inject] 
        public RewardCollectorsController(List<IRewardCollector> rewardCollectors)
        {
            m_RewardCollectorsService = new RewardCollectorsService(rewardCollectors);
        }

        public void CollectReward(IReward reward)
        {
            m_RewardCollectorsService.CollectReward(reward);
        }
        
        public void CollectRewards(IEnumerable<IReward> rewards)
        {
            foreach (var reward in rewards)
            {
                m_RewardCollectorsService.CollectReward(reward);
            }
        }
    }
}