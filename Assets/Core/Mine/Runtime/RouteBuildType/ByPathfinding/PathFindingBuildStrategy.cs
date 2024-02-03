using System;
using System.Collections.Generic;
using System.Linq;
using Core.Mine.Runtime.Point;
using Core.Mine.Runtime.Point.Base;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.ByPathfinding
{
    public class PathFindingBuildStrategy : IRouteBuildStrategy
    {
        private readonly Dictionary<PathfindingAlgorithmType, IPathfindingAlgorithm> m_PathfindingAlgorithms;

        public PathFindingBuildStrategy()
        {
            m_PathfindingAlgorithms = new Dictionary<PathfindingAlgorithmType, IPathfindingAlgorithm>
            {
                [PathfindingAlgorithmType.Dijkstras] = new DijkstrasAlgorithm()
            };
        }

        public IReadOnlyList<IPoint> BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType)
        {
            if (routeBuildType is ByPathfindingBuildType pathfindingBuildType)
            {
                var navigationMap = mineMap.NavigationMap;
                var resourcePoints = new List<IResourcePoint>();
                foreach (var point in navigationMap.Keys)
                {
                    if (point is IResourcePoint resourcePoint)
                    {
                        resourcePoints.Add(resourcePoint);
                    }
                }
                
                var pathfindingAlgorithm = 
                    m_PathfindingAlgorithms.GetValueOrDefault(pathfindingBuildType.PathfindingAlgorithmType);
                
                var path = new List<IPoint>();

                var from = FindBasePoint(navigationMap);
                
                foreach (var resourcePoint in resourcePoints)
                {
                    var routeToResourcePoint 
                        = pathfindingAlgorithm.Find(navigationMap, from, resourcePoint);

                    Combine(path, routeToResourcePoint);
                    
                    if (pathfindingBuildType.ReturnToBaseAfterResourcePoint)
                    {
                        var copy = routeToResourcePoint.ToList();
                        copy.Reverse();
                        Combine(path, copy);
                        from = path[0];
                        continue;
                    }
                    
                    from = path[path.Count - 1];
                }

                if (!pathfindingBuildType.ReturnToBaseAfterResourcePoint)
                {
                    var basePoint = FindBasePoint(navigationMap);
                    var lastPoint = path.Last();
                    var routeToBase = pathfindingAlgorithm.Find(navigationMap, lastPoint, basePoint);
                    Combine(path, routeToBase);
                }
                
                return path;
            }

            Debug.LogError("Can't match types");
            return null;
        }

        private IPoint FindBasePoint(IReadOnlyDictionary<IPoint,IReadOnlyList<IPoint>> navigationMap)
        { 
            return navigationMap.Keys.FirstOrDefault(p => p.GetType() == typeof(BasePoint));
        }

        private List<IPoint> Combine(List<IPoint> path, List<IPoint> additional)
        {
            if (path.LastOrDefault() == additional.FirstOrDefault())
            {
                for (int i = 1; i < additional.Count; i++)
                {
                    path.Add(additional[i]);
                }
            }
            else
            {
                path.AddRange(additional);
            }

            return path;
        }
    }
}