using Common.Navigation.Runtime.Waypoint;

namespace Common.Navigation.Runtime
{
    public interface IRouteNavigator
    {
        RouteInfo BuildRoute(IWaypoint startPoint, IWaypoint endPoint);
    }
}