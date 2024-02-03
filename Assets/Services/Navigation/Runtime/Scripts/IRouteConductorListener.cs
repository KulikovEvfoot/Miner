using Services.Navigation.Runtime.Scripts.Transfer;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IRouteConductorListener
    {
        void Tick(RouteConductorResult routeTickInfo);
    }
}