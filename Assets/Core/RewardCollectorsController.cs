using System.Collections.Generic;
using Common.Currency.Runtime.Rewards;
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
    }
}