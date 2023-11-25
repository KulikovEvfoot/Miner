using Common.Currency;
using Common.Currency.Runtime;
using Common.Currency.Runtime.Calculators;

namespace Core.Currency.Runtime.Gold
{
    public class GoldCurrencyController : ICurrencyController
    {
        private readonly ICurrencyCalculator m_CurrencyCalculator = new CurrencyCalculator();
        private readonly GoldCurrencyData m_GoldCurrencyData;

        public GoldCurrencyController(GoldCurrencyData goldCurrencyData)
        {
            m_GoldCurrencyData = goldCurrencyData;
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
            return true;
        }

        public bool SubtractValue(long amount)
        {
            var startValue = m_GoldCurrencyData.GetValue();
            var result = m_CurrencyCalculator.AddValue(startValue, amount);
            if (!result.IsSuccess)
            {
                return false;
            }

            m_GoldCurrencyData.SetValue(result.NewValue);
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
    }
}