namespace Common.Wallet.Runtime.Calculators
{
    public class CalculationResult
    {
        public long NewValue { get; }
        public bool IsSuccess { get; }

        public CalculationResult(long newValue, bool isSuccess)
        {
            NewValue = newValue;
            IsSuccess = isSuccess;
        }
    }
}
