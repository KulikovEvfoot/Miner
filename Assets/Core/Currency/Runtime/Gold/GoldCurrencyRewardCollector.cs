using System;
using Common.Currency.Runtime.Rewards;
using UnityEngine;

namespace Core.Currency.Runtime.Gold
{
    public class GoldCurrencyRewardCollector : IRewardCollector
    {
        private readonly GoldCurrencyController m_GoldCurrencyController;

        public GoldCurrencyRewardCollector(GoldCurrencyController goldCurrencyController)
        {
            m_GoldCurrencyController = goldCurrencyController;
        }

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