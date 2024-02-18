using System.Collections.Generic;
using System.Linq;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.Defined
{
    public class DefinedRouteBuildStrategy : IRouteBuildStrategy
    {
        public IRoute BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType)
        {
            if (routeBuildType is DefinedRouteBuildType definedRouteBuildType)
            {
                var highwayIndexes = definedRouteBuildType.Highways;
                var minePoints = mineMap.Points;

                var highways = new List<IHighway>(){};
                foreach (var indexes in highwayIndexes)
                {
                    var points = new List<IPoint>();
                    points.AddRange(indexes.PathIndexes.Select(index => minePoints[index]));
                    highways.Add(new Highway(points));
                }

                var route = new Route(highways);
                return route;
            }

            Debug.LogError("Can't match types");
            return null;
        }
    }
}