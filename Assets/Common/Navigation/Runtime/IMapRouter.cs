using System.Collections.Generic;

namespace Common.Navigation.Runtime
{
    public interface IMapRouter
    {
        List<Route> Routes { get; }
        List<T> FindAllWaypointsByType<T>();
    }
}