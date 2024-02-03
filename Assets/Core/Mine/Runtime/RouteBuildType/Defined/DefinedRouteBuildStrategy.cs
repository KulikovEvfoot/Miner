using System.Collections.Generic;
using System.Linq;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.Defined
{
    public class DefinedRouteBuildStrategy : IRouteBuildStrategy
    {
        public IReadOnlyList<IPoint> BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType)
        {
            if (routeBuildType is DefinedRouteBuildType definedRouteBuildType)
            {
                var pathIndexes = definedRouteBuildType.NavigationPathIndexes;
                var minePoints = mineMap.Points;
                return pathIndexes.Select(pathIndex => minePoints[pathIndex]).ToList();
            }

            Debug.LogError("Can't match types");
            return null;
        }
    }
}