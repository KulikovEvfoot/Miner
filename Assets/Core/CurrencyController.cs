using System.Collections.Generic;
using Common.Currency.Runtime;
using Common.Currency.Runtime.Rewards;
using Core.Currency.Runtime.Gold;
using UnityEngine;

namespace Core
{
    public class CurrencyController : MonoBehaviour
    {
        private readonly GoldCurrencyData m_GoldCurrencyData = new GoldCurrencyData();
        
        private GoldCurrencyController m_GoldCurrencyController;
        private GoldCurrencyRewardCollector m_GoldCurrencyRewardCollector;
        private List<IRewardCollector> m_RewardCollectors;

        public ICurrencyController GoldCurrencyController => m_GoldCurrencyController;
        public List<IRewardCollector> RewardCollectors => m_RewardCollectors;

        public void Init()
        {
            m_GoldCurrencyController = new GoldCurrencyController(m_GoldCurrencyData);
            m_GoldCurrencyRewardCollector = new GoldCurrencyRewardCollector(m_GoldCurrencyController);
            
            m_RewardCollectors = new List<IRewardCollector>
            {
                m_GoldCurrencyRewardCollector
            };
        }
    }
}