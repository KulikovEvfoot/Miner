using System;
using UnityEngine;

namespace Core.Currency.Runtime.Gold
{
    [Serializable]
    public class GoldReward : IResourceReward
    {
        [SerializeField] private long m_Value;

        public long Value => m_Value;

        public GoldReward(long value)
        {
            m_Value = value;
        }
    }
}