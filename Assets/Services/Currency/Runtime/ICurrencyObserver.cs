namespace Services.Currency.Runtime
{
    public interface ICurrencyObserver
    {
        public void NotifyOnValueChanged(long value);
    }
}