using Common;
using Services.Currency.Runtime;
using Services.Currency.Runtime.Calculators;

namespace Core.Currency.Runtime.Gold
{
    public class GoldCurrencyController : ICurrencyController
    {
        private readonly ICurrencyCalculator m_CurrencyCalculator = new CurrencyCalculator();
        private readonly GoldCurrencyData m_GoldCurrencyData;
        private readonly EventProducer<ICurrencyObserver> m_CurrencyEventProducer;
        
        public IEventProducer<ICurrencyObserver> CurrencyEventProducer => m_CurrencyEventProducer;

        public GoldCurrencyController(GoldCurrencyData goldCurrencyData)
        {
            m_GoldCurrencyData = goldCurrencyData;
            m_CurrencyEventProducer = new EventProducer<ICurrencyObserver>();
        }
        
        public bool AddValue(long amount)
        {
            var startValue = m_GoldCurrencyData.GetValue();
            var result = m_CurrencyCalculator.AddValue(startValue, amount);
            if (!result.IsSuccess)
            {
                return false;
            }

            m_GoldCurrencyData.SetValue(result.NewValue);
            NotifyOnValueChanged();
            return true;
        }

        public bool SubtractValue(long amount)
        {
            var startValue = m_GoldCurrencyData.GetValue();
            var result = m_CurrencyCalculator.SubtractValue(startValue, amount);
            if (!result.IsSuccess)
            {
                return false;
            }

            m_GoldCurrencyData.SetValue(result.NewValue);
            NotifyOnValueChanged();
            return true;
        }

        public long GetValue()
        {
            return m_GoldCurrencyData.GetValue();
        }

        public bool CanSub(long amount)
        {
            if (amount < 0)
            {
                return false;
            }
            
            return m_GoldCurrencyData.GetValue() >= amount;
        }

        private void NotifyOnValueChanged()
        {
            var value = m_GoldCurrencyData.GetValue();
            m_CurrencyEventProducer.NotifyAll(obs => obs.NotifyOnValueChanged(value));
        }
    }
}