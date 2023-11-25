namespace Common.Wallet.Runtime
{
    public interface ICurrencyController
    {
        bool AddValue(long amount);
        bool SubtractValue(long amount);
        long GetValue();
        bool CanSub(long amount);
    }
}