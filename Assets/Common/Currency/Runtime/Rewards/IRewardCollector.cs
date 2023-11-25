using System;

namespace Common.Currency.Runtime.Rewards
{
    public interface IRewardCollector
    {
        Type GetRewardType();
        void CollectReward(IReward reward);
    }
}