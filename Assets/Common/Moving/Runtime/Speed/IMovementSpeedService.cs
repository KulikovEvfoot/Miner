namespace Common.Moving.Runtime.Speed
{
    public interface IMovementSpeedService
    {
        float Speed { get; }
        void Inc();
        void Dec();
    }
}