using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class SamplePriceConfig
    {
        [SerializeField] private long m_CreateMinerPrice;
        [SerializeField] private long m_SpeedUpMinersPrice;

        public long CreateMinerPrice => m_CreateMinerPrice;
        public long SpeedUpMinersPrice => m_SpeedUpMinersPrice;
    }
}