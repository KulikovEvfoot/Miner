using Common.Currency.Runtime.Rewards;

namespace Core.Currency.Runtime.Gold
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