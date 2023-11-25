using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Wallet.Runtime.Rewards
{
    public class RewardCollectorsService
    {
        private readonly Dictionary<Type, IRewardCollector> m_CollectorsMap;
        
        public RewardCollectorsService(List<IRewardCollector> rewardCollectors)
        {
            m_CollectorsMap = ConfigureMap(rewardCollectors);
        }
        
        public void CollectReward(IReward reward)
        {
            if (reward == null)
            {
                Debug.LogError($"{nameof(RewardCollectorsService)} >>> Reward is null");
                return;
            }
            
            var type = reward.GetType();
            if (!m_CollectorsMap.ContainsKey(type))
            {
                Debug.LogError($"{nameof(RewardCollectorsService)} >>> Reward {nameof(type)}, does not contains");
                return;
            }
            
            m_CollectorsMap[type].CollectReward(reward);
        }

        private Dictionary<Type, IRewardCollector> ConfigureMap(List<IRewardCollector> rewardCollectors)
        {
            var map = new Dictionary<Type, IRewardCollector>();
            foreach (var collector in rewardCollectors)
            {
                var type = collector.GetRewardType();
                if (map.ContainsKey(type))
                {
                    Debug.LogError($"Map<{nameof(Type)}, {nameof(IRewardCollector)}> already contain" +
                                   $"type {type}, duplicate");
                    continue;
                }
                
                map.Add(type, collector);
            }

            return map;
        }
    }
}