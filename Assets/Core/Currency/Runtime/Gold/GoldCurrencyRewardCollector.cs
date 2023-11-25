using System;
using Common.Wallet.Runtime.Rewards;
using Core.Currency.Runtime;
using Core.WalletFeature.Currency.Hard;

namespace Core.Wallet.Runtime.Gold
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
                return;
            }

            if (reward is GoldReward currencyReward)
            {
                m_GoldCurrencyController.AddValue(currencyReward.Value);
            }
        }
    }
}