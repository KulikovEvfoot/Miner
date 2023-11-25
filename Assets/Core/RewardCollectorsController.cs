using System.Collections.Generic;
using Common.Wallet.Runtime.Rewards;
using Core.Wallet.Runtime.Gold;
using Core.WalletFeature.Currency.Hard;
using UnityEngine;

namespace Core.Currency.Runtime
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