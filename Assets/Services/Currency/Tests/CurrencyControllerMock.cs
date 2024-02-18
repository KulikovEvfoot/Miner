using Common;
using Services.Currency.Runtime;
using Services.Currency.Runtime.Calculators;

namespace Services.Currency.Tests
{
    public class CurrencyControllerMock : ICurrencyController
    {
        private readonly ICurrencyCalculator m_CurrencyCalculator = new CurrencyCalculator();
        private readonly CurrencyDataMock m_CurrencyDataMock = new();
        private readonly EventProducer<ICurrencyObserver> m_CurrencyEventProducer = new();

        public IEventProducer<ICurrencyObserver> EventProducer => m_CurrencyEventProducer;

        public bool AddValue(long amount)
        {
            var startValue = m_CurrencyDataMock.GetValue();
            var result = m_CurrencyCalculator.AddValue(startValue, amount);
            if (!result.IsSuccess)
            {
                return false;
            }

            m_CurrencyDataMock.SetValue(result.NewValue);
            NotifyOnValueChanged();
            return true;
        }

        public bool SubtractValue(long amount)
        {
            var startValue = m_CurrencyDataMock.GetValue();
            var result = m_CurrencyCalculator.SubtractValue(startValue, amount);
            if (!result.IsSuccess)
            {
                return false;
            }

            m_CurrencyDataMock.SetValue(result.NewValue);
            NotifyOnValueChanged();
            return true;
        }

        public long GetValue()
        {
            return m_CurrencyDataMock.GetValue();
        }

        public bool CanSub(long amount)
        {
            if (amount < 0)
            {
                return false;
            }
            
            return m_CurrencyDataMock.GetValue() >= amount;
        }
        
        private void NotifyOnValueChanged()
        {
            var value = m_CurrencyDataMock.GetValue();
            m_CurrencyEventProducer.NotifyAll(obs => obs.NotifyOnValueChanged(value));
        }
    }
}