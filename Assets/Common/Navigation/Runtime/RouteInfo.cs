using System.Collections.Generic;
using Common.Navigation.Runtime.Waypoint;

namespace Common.Navigation.Runtime
{
    public class RouteInfo
    {
        public List<IWaypoint> Waypoints { get; }
        public float Lenght { get; }

        public RouteInfo(List<IWaypoint> waypoints, float lenght)
        {
            Waypoints = waypoints;
            Lenght = lenght;
        }
    }
}