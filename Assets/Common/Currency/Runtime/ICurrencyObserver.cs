namespace Common.Currency.Runtime
{
    public interface ICurrencyObserver
    {
        public void NotifyOnValueChanged(long value);
    }
}