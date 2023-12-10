namespace Services.Navigation.Runtime.Scripts.Transfer.Speed
{
    public interface ISpeedService
    {
        float Speed { get; }
        void Inc();
        void Dec();
        bool CanInc();
        bool CanDec();
    }
}