namespace Common.Wallet.Runtime.Calculators
{
    public class CurrencyCalculator : ICurrencyCalculator
    {
        public CalculationResult AddValue(long startValue, long amount)
        {
            long newValue;
            if (startValue >= 0 && amount >= 0 && startValue + amount < 0)
            {
                newValue = long.MaxValue;
                return new CalculationResult(newValue, true);
            }

            if (amount < 0)
            {
                return new CalculationResult(default, false);
            }

            newValue = startValue + amount;
            return new CalculationResult(newValue, true);
        }

        public CalculationResult SubtractValue(long startValue, long amount)
        {
            if (startValue >= 0 && amount >= 0 && startValue - amount < 0)
            {
                return new CalculationResult(default, false);
            }

            if (amount < 0)
            {
                return new CalculationResult(default, false);
            }
            
            var newValue = startValue - amount;
            return new CalculationResult(newValue, true);
        }
    }
}