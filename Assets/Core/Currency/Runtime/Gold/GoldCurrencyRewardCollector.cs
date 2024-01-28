using System;
using Services.Currency.Runtime.Rewards;
using UnityEngine;
using Zenject;

namespace Core.Currency.Runtime.Gold
{
    public class GoldCurrencyRewardCollector : IRewardCollector
    {
        [Inject] private GoldCurrencyController m_GoldCurrencyController;
        
        public Type GetRewardType()
        {
            return typeof(GoldReward);
        }

        public void CollectReward(IReward reward)
        {
            if (reward == null)
            {
                Debug.LogError($"{nameof(GoldCurrencyRewardCollector)} >>> Entry reward is null");
                return;
            }

            if (reward is GoldReward currencyReward)
            {
                m_GoldCurrencyController.AddValue(currencyReward.Value);
                return;
            }
            
            Debug.LogError($"{nameof(GoldCurrencyRewardCollector)} >>> Entry {reward.GetType()} is not {typeof(GoldReward)}");
        }
    }
}