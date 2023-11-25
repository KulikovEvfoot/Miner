namespace Common.EventProducer.Runtime
{
    public interface IEventProducer<T> 
    {
        void Attach(T observer);
        void Detach(T observer);
    }
}