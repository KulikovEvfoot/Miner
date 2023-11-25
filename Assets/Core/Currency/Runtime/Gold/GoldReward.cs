﻿using Common.Wallet.Runtime.Rewards;

namespace Core.Currency.Runtime
{
    public class GoldReward : IReward
    {
        public long Value { get; }

        public GoldReward(long value)
        {
            Value = value;
        }
    }
}