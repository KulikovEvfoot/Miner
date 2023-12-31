using System;
using Common.EventProducer.Runtime;

namespace Common.Currency.Runtime
{
    public interface ICurrencyController
    {
        bool AddValue(long amount);
        bool SubtractValue(long amount);
        long GetValue();
        bool CanSub(long amount);
        IEventProducer<ICurrencyObserver> CurrencyEventProducer { get; }
    }
}