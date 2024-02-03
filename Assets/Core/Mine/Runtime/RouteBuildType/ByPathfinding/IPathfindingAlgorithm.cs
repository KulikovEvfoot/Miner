using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.RouteBuildType.ByPathfinding
{
    public interface IPathfindingAlgorithm
    {
        List<IPoint> Find(IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> route, IPoint from, IPoint to);
    }
}