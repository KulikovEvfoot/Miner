using Common.Wallet.Runtime.Rewards;

namespace Core.Job.Runtime
{
    public interface IResourceDeposit
    { 
        IReward Reward { get; }
        long Count { get; }
    }
}