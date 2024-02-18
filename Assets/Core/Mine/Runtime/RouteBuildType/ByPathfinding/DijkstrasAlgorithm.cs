using System.Collections.Generic;
using System.Linq;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.ByPathfinding
{
    public class DijkstrasAlgorithm : IPathfindingAlgorithm
    {
        public List<IPoint> Find(IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> route, IPoint from, IPoint to)
        {
            var graph = new Dictionary<IPoint, Dictionary<IPoint, float>>();
            
            var baseNeighbours = route.GetValueOrDefault(from);
            graph.Add(from, new Dictionary<IPoint, float>());
            foreach (var baseNeighbour in baseNeighbours)
            {
                var range = NavigationUtils.GetTransitionLength(from, baseNeighbour);
                graph[from].Add(baseNeighbour, range);
            }
            
            foreach (var pair in route)
            {
                if (graph.ContainsKey(pair.Key))
                {
                    continue;
                }
                
                graph.Add(pair.Key, new Dictionary<IPoint, float>());
                foreach (var neighbour in pair.Value)
                {
                    var range = NavigationUtils.GetTransitionLength(pair.Key, neighbour);
                    graph[pair.Key].Add(neighbour, range);
                }
            }

            var path = Build(graph, to);
            return path;
        }
        
        private List<IPoint> Build(Dictionary<IPoint, Dictionary<IPoint, float>> graph, IPoint endPoint)
        {
            var firstPair = graph.First();
            var parents = new Dictionary<IPoint, IPoint>();
            var costs = new Dictionary<IPoint, float>();
            foreach (var firstNeighbor in firstPair.Value)
            {
                parents.Add(firstNeighbor.Key, firstPair.Key);
                costs.Add(firstNeighbor.Key, firstNeighbor.Value);
            }

            foreach (var mainMapKey in graph.Keys)
            {
                if (mainMapKey.Equals(firstPair.Key))
                {
                    continue;
                }

                if (parents.ContainsKey(mainMapKey))
                {
                    continue;
                }

                parents.Add(mainMapKey, default);
                costs.Add(mainMapKey, float.MaxValue);
            }

            var processed = new HashSet<IPoint>();
            for (int i = 0; i < graph.Count - 1; i++)
            {
                var node = FindLowestCostNode(costs, processed);
                var cost = costs[node];
                var neighbors = graph[node];
                foreach (var neighbor in neighbors)
                {
                    if (!costs.ContainsKey(neighbor.Key))
                    {
                        continue;
                    }

                    var newCost = cost + neighbor.Value;
                    var oldCost = costs[neighbor.Key];
                    if (oldCost > newCost)
                    {
                        costs[neighbor.Key] = newCost;
                        parents[neighbor.Key] = node;
                    }
                }

                processed.Add(node);
            }

            var resultPath = new List<IPoint>();
            var length = 0f;
            var searchablePair = parents.FirstOrDefault(p => p.Key.Equals(endPoint));
            if (searchablePair.Equals(new KeyValuePair<IPoint, IPoint>()))
            {
                Debug.LogError($"Can't build path");
                return null;
            }
            
            resultPath.Add(searchablePair.Key);
            length += costs[searchablePair.Key];
            var lastParent = searchablePair.Value;
            while (true)
            {
                resultPath.Add(lastParent);
                if (lastParent.Equals(firstPair.Key))
                {
                    break;
                }
                
                length += costs[lastParent];
                lastParent = parents[lastParent];
            }

            resultPath.Reverse();
            
            return resultPath;
        }

        private IPoint FindLowestCostNode(IDictionary<IPoint, float> costs, HashSet<IPoint> processed)
        {
            var lowesCost = float.MaxValue;
            IPoint waypoint = null;
            foreach (var node in costs)
            {
                var cost = node.Value;
                if (cost < lowesCost && !processed.Contains(node.Key))
                {
                    lowesCost = cost;
                    waypoint = node.Key;
                }
            }

            return waypoint;
        }
    }
}