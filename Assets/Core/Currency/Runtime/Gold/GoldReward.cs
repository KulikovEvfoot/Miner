using System;
using Services.Currency.Runtime.Rewards;
using UnityEngine;

namespace Core.Currency.Runtime.Gold
{
    [Serializable]
    public class GoldReward : IReward
    {
        [SerializeField] private long m_Value;

        public long Value => m_Value;
    }
}