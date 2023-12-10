namespace Services.Currency.Runtime.Calculators
{
    public interface ICurrencyCalculator
    {
        CalculationResult AddValue(long startValue, long amount);
        CalculationResult SubtractValue(long startValue, long amount);
    }
}
