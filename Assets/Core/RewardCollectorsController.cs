using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using UnityEngine;

namespace Core
{
    public class RewardCollectorsController : MonoBehaviour
    {
        private RewardCollectorsService m_RewardCollectorsService;
        
        public void Init(List<IRewardCollector> rewardCollectors)
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