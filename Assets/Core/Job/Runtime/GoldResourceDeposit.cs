using System;
using Common.Wallet.Runtime.Rewards;
using Core.Currency.Runtime;
using UnityEngine;

namespace Core.Job.Runtime
{
    [Serializable]
    public class GoldResourceDeposit : IResourceDeposit
    {
        [SerializeField] private long m_Count;

        public IReward Reward => new GoldReward(m_Count);
        public long Count => m_Count;
    }
}