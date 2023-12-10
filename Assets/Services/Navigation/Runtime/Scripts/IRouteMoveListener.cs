namespace Services.Navigation.Runtime.Scripts
{
    public interface IRouteMoveListener : IOnTransferCompleteListener, IOnRouteCompleteListener
    {
    }

    public interface IOnTransferCompleteListener
    {
        void NotifyOnTransferComplete(ITransition transition);
    }

    public interface IOnRouteCompleteListener
    {
        void NotifyOnRouteComplete(float time);
    }
}