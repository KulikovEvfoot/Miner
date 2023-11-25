using System.Collections.Generic;
using Common.Currency.Runtime.Rewards;
using Core.Currency.Runtime.Gold;
using UnityEngine;

namespace Core
{
    public class RewardCollectorsController : MonoBehaviour
    {
        private RewardCollectorsService m_RewardCollectorsService;
        
        public void Init()
        {
            var goldCurrencyData = new GoldCurrencyData();
            var goldCurrencyController = new GoldCurrencyController(goldCurrencyData);
            var goldCurrencyRewardCollector = new GoldCurrencyRewardCollector(goldCurrencyController);
            
            var rewardCollectors = new List<IRewardCollector> { goldCurrencyRewardCollector };
            m_RewardCollectorsService = new RewardCollectorsService(rewardCollectors);
        }

        public void CollectReward(IReward reward)
        {
            m_RewardCollectorsService.CollectReward(reward);
        }
    }
}