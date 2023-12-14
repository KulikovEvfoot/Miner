using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IRouteMoveListener : IRouteTickListener, ITransitionCompleteListener, IRouteCompleteListener
    {
    }

    public interface IRouteTickListener
    {
        void Tick(RouteTickInfo routeTickInfo);
    }
    
    public interface ITransitionCompleteListener
    {
        void NotifyOnTransitionComplete(IEnumerable<ITransition> passedTransitions);
    }

    public interface IRouteCompleteListener
    {
        void NotifyOnRouteComplete(IEnumerable<ITransition> route, int countOfCompletedLoops);
    }
}