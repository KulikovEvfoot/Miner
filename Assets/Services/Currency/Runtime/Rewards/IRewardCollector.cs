using System;

namespace Services.Currency.Runtime.Rewards
{
    public interface IRewardCollector
    {
        Type GetRewardType();
        void CollectReward(IReward reward);
    }
}