using System;

namespace Core.Currency.Runtime.Gold
{
    [Serializable]
    public class GoldCurrencyData
    {
        private long m_Value;
        
        public long GetValue()
        {
            return m_Value;
        }
        
        public void SetValue(long newValue)
        {
            m_Value = newValue;
        }
    }
}