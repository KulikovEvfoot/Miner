namespace Services.Currency.Tests
{
    public class CurrencyDataMock
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