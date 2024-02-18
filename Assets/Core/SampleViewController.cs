using System.Threading.Tasks;
using Common;
using Core.Currency.Runtime.Gold;
using Core.ResourceExtraction;
using Services.AssetLoader.Runtime;
using Services.Currency.Runtime;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Core
{
    public class SampleViewController : IInitPhase, ICurrencyObserver
    {
        [Inject] private SampleGameConfig m_SampleGameConfig;
        [Inject] private CanvasProvider m_CanvasProvider;
        [Inject] private GoldCurrencyController m_GoldCurrencyController;
        [Inject] private AddressablesAssetLoader m_AssetLoader;
        [Inject] private ResourcesExtractionController m_ResourcesExtractionController;
        
        private SampleView m_SampleView;
        
        private IEventProducer<ICurrencyObserver> m_CurrencyEventProducer;
        
        public async Task Init()
        {
            m_SampleView = await m_AssetLoader.
                InstantiateAsync<SampleView>(m_SampleGameConfig.SampleViewReference, new InstantiationParameters());
            
            m_SampleView.Init(CreateMiner, SpeedUpMiners);

            var viewParent = m_CanvasProvider.UICanvas.transform;
            m_SampleView.transform.SetParentAndNormalize(viewParent);
                
            m_SampleView.SetGoldCurrencyText(m_GoldCurrencyController.GetValue().ToString());
            m_SampleView.SetCreateMinerPriceText(m_SampleGameConfig.SamplePriceConfig.CreateMinerPrice.ToString());
            m_SampleView.SetSpeedUpMinersPriceText(m_SampleGameConfig.SamplePriceConfig.SpeedUpMinersPrice.ToString());
            
            m_GoldCurrencyController.EventProducer.Attach(this);
        }


        private void CreateMiner()
        {
            var price = m_SampleGameConfig.SamplePriceConfig.CreateMinerPrice;
            if (!m_GoldCurrencyController.CanSub(price))
            {
                Debug.Log($"{nameof(SampleViewController)} >>> Not enough currency in {m_GoldCurrencyController.GetType()}");
                return;
            }

            m_GoldCurrencyController.SubtractValue(price);
            m_ResourcesExtractionController.CreateMiner();
        }

        private void SpeedUpMiners()
        {
            var price = m_SampleGameConfig.SamplePriceConfig.CreateMinerPrice;
            if (!m_GoldCurrencyController.CanSub(price))
            {
                Debug.Log($"{nameof(SampleViewController)} >>> Not enough currency in {m_GoldCurrencyController.GetType()}");
                return;
            }

            if (!m_ResourcesExtractionController.CanSpeedUpMiners())
            {
                Debug.Log($"{nameof(SampleViewController)} >>> Miners has max speed");
                return;
            }

            m_GoldCurrencyController.SubtractValue(price);
            m_ResourcesExtractionController.SpeedUpMiners();
        }

        private void OnDestroy()
        {
            m_GoldCurrencyController.EventProducer.Detach(this);
        }

        public void NotifyOnValueChanged(long value)
        {
            m_SampleView.SetGoldCurrencyText(value.ToString());
        }
    }
}