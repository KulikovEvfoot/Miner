using System;
using Common.Currency.Runtime;
using Common.EventProducer.Runtime;
using UnityEngine;

namespace Core
{
    public class SampleViewController : MonoBehaviour, ICurrencyObserver
    {
        [SerializeField] private SampleView m_SampleView;
        
        private MinersController m_MinersController;
        private ICurrencyController m_GoldCurrencyController;
        private SamplePriceConfig m_SamplePriceConfig;
        private IEventProducer<ICurrencyObserver> m_CurrencyEventProducer;

        public void Init(
            MinersController minersController, 
            ICurrencyController goldCurrencyController, 
            SamplePriceConfig samplePriceConfig)
        {
            m_MinersController = minersController;
            m_GoldCurrencyController = goldCurrencyController;
            m_SamplePriceConfig = samplePriceConfig;
            
            m_CurrencyEventProducer = goldCurrencyController.CurrencyEventProducer;
            m_CurrencyEventProducer.Attach(this);

            m_SampleView.Init(CreateMiner, SpeedUpMiners);
            
            m_SampleView.SetGoldCurrencyText(m_GoldCurrencyController.GetValue().ToString());
            m_SampleView.SetCreateMinerPriceText(samplePriceConfig.CreateMinerPrice.ToString());
            m_SampleView.SetSpeedUpMinersPriceText(samplePriceConfig.SpeedUpMinersPrice.ToString());
        }

        private void CreateMiner()
        {
            var price = m_SamplePriceConfig.CreateMinerPrice;
            if (!m_GoldCurrencyController.CanSub(price))
            {
                Debug.Log($"{nameof(SampleViewController)} >>> Not enough currency in {m_GoldCurrencyController.GetType()}");
                return;
            }

            m_GoldCurrencyController.SubtractValue(price);
            m_MinersController.CreateMiner();
        }

        private void SpeedUpMiners()
        {
            var price = m_SamplePriceConfig.SpeedUpMinersPrice;
            if (!m_GoldCurrencyController.CanSub(price))
            {
                Debug.Log($"{nameof(SampleViewController)} >>> Not enough currency in {m_GoldCurrencyController.GetType()}");
                return;
            }

            m_GoldCurrencyController.SubtractValue(price);
            m_MinersController.SpeedUpMiners();
        }

        private void OnDestroy()
        {
            m_CurrencyEventProducer.Detach(this);
        }

        public void NotifyOnValueChanged(long value)
        {
            m_SampleView.SetGoldCurrencyText(value.ToString());
        }
    }
}