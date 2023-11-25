using Common.Navigation.Runtime.Waypoint;

namespace Common.Navigation.Runtime
{
    public interface IRouteBuilder
    {
        RouteInfo BuildRoute(IWaypoint startPoint, IWaypoint endPoint);
    }
}