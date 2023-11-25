using System.Collections.Generic;

namespace Common.Navigation.Runtime
{
    public interface IMapRouter
    {
        Route Map { get; }
        List<T> FindAllWaypointsByType<T>();
    }
}