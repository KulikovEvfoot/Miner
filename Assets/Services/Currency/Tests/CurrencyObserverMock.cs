using Services.Currency.Runtime;

namespace Services.Currency.Tests
{
    public class CurrencyObserverMock : ICurrencyObserver
    {
        public long Value { get; private set; }
        public void NotifyOnValueChanged(long value)
        {
            Value = value;
        }
    }
}