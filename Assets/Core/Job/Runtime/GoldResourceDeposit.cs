using System;
using Common.Currency.Runtime.Rewards;
using Core.Currency.Runtime.Gold;
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