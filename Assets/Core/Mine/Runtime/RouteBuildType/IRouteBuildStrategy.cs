using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.RouteBuildType
{
    public interface IRouteBuildStrategy
    {
        IReadOnlyList<IPoint> BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType);
    }
}