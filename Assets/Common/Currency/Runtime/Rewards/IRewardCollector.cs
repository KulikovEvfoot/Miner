using System;

namespace Common.Wallet.Runtime.Rewards
{
    public interface IRewardCollector
    {
        Type GetRewardType();
        void CollectReward(IReward reward);
    }
}