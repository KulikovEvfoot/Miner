namespace Common
{
    public interface IEventProducer<T> 
    {
        void Attach(T observer);
        void Detach(T observer);
    }
}