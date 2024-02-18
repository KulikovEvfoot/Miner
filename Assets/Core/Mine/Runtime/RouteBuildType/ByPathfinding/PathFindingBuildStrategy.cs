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

        public IRoute BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType)
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

                if (pathfindingBuildType.ReturnToBaseAfterResourcePoint)
                {
                    return ReturnToBaseAlgorithm(resourcePoints, pathfindingAlgorithm, navigationMap);
                }

                return DefaultSearchingAlgorithm(resourcePoints, pathfindingAlgorithm, navigationMap);
            }

            Debug.LogError("Can't match types");
            return null;
        }

        private IRoute DefaultSearchingAlgorithm(List<IResourcePoint> resourcePoints,
            IPathfindingAlgorithm pathfindingAlgorithm,
            IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> navigationMap)
        {
            var points = new List<IPoint>();
            var basePoint = FindBasePoint(navigationMap);
            var from = basePoint;
            foreach (var resourcePoint in resourcePoints)
            {
                var routeToResourcePoint 
                    = pathfindingAlgorithm.Find(navigationMap, from, resourcePoint);
                
                from = routeToResourcePoint[^1];

                points.AddRange(routeToResourcePoint);
            }
            
            points.AddRange(pathfindingAlgorithm.Find(navigationMap, from, basePoint));
            
            var highway = new Highway(points);
            var highways = new List<IHighway> { highway };
            return new Route(highways);
        }

        private IRoute ReturnToBaseAlgorithm(
            List<IResourcePoint> resourcePoints,
            IPathfindingAlgorithm pathfindingAlgorithm,
            IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> navigationMap)
        {
            var highways = new List<IHighway>();
            var from = FindBasePoint(navigationMap);
            foreach (var resourcePoint in resourcePoints)
            {
                var routeToResourcePoint 
                    = pathfindingAlgorithm.Find(navigationMap, from, resourcePoint);
                    
                var mirroredCopy = new IPoint[routeToResourcePoint.Count - 1];

                for (int i = 0; i < routeToResourcePoint.Count - 1; i++)
                {
                    mirroredCopy[i] = routeToResourcePoint[routeToResourcePoint.Count - 2 - i];
                }
                
                routeToResourcePoint.AddRange(mirroredCopy);
                highways.Add(new Highway(routeToResourcePoint));
            }

            return new Route(highways);
        }
        
        private IPoint FindBasePoint(IReadOnlyDictionary<IPoint,IReadOnlyList<IPoint>> navigationMap)
        { 
            return navigationMap.Keys.FirstOrDefault(p => p.GetType() == typeof(BasePoint));
        }
    }
}