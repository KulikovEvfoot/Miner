using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class SampleView : MonoBehaviour
    {
        [SerializeField] private Button m_CreateMinerButton;
        [SerializeField] private Text m_CreateMinerPrice;
        
        [Space]
        [SerializeField] private Button m_SpeedUpMinersButton;
        [SerializeField] private Text m_SpeedUpMinersPrice;
        
        [Space]
        [SerializeField] private Text m_GoldCurrencyText;
        
        private Action m_CreateMiner;
        private Action m_SpeedUpMiners;

        public void Init(Action createMiner, Action speedUpMiners)
        {
            m_CreateMiner = createMiner;
            m_SpeedUpMiners = speedUpMiners;
            m_CreateMinerButton.onClick.AddListener(InvokeCreateMinerCall);
            m_SpeedUpMinersButton.onClick.AddListener(InvokeSpeedUpMinersCall);
        }

        public void SetGoldCurrencyText(string text)
        {
            m_GoldCurrencyText.text = text;
        }
        
        public void SetCreateMinerPriceText(string text)
        {
            m_CreateMinerPrice.text = text;
        }
        
        public void SetSpeedUpMinersPriceText(string text)
        {
            m_SpeedUpMinersPrice.text = text;
        }
        
        private void InvokeCreateMinerCall()
        {
            m_CreateMiner?.Invoke();
        }     
        
        private void InvokeSpeedUpMinersCall()
        {
            m_SpeedUpMiners?.Invoke();
        }

        private void OnDestroy()
        {
            m_CreateMinerButton.onClick.RemoveListener(InvokeCreateMinerCall);
            m_SpeedUpMinersButton.onClick.RemoveListener(InvokeSpeedUpMinersCall);
        }
    }
}