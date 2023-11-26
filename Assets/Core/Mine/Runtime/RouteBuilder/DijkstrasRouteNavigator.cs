using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuilder
{
    public class DijkstrasRouteNavigator : IRouteNavigator
    {
        private readonly IMapRouter m_MapRouter;

        public DijkstrasRouteNavigator(IMapRouter mapRouter)
        {
            m_MapRouter = mapRouter;
        }

        public RouteInfo BuildRoute(IWaypoint startPoint, IWaypoint endPoint)
        {
            var graph = CreateGraph(startPoint);
            var result = DijkstrasAlgorithm(graph, endPoint);
            return result;
        }

        //TODO: оптимизировать создание графа
        private Dictionary<IWaypoint, Dictionary<IWaypoint, float>> CreateGraph(IWaypoint startPoint)
        {
            var map = m_MapRouter.Map;
            var graph = new Dictionary<IWaypoint, Dictionary<IWaypoint, float>>();
            var transitions = map.Waypoints
                .SelectMany(w => w.Transitions)
                .Where(t => t.From.Equals(startPoint))
                .ToList();
            
            var temp = new List<ITransition>();
            while (true)
            {
                foreach (var transition in transitions)
                {
                    if (!graph.ContainsKey(transition.From))
                    {
                        graph.Add(transition.From, new Dictionary<IWaypoint, float>());
                    }

                    if (transition.To.Equals(startPoint))
                    {
                        continue;
                    }

                    if (graph[transition.From].ContainsKey(transition.To))
                    {
                        continue;
                    }
                        
                    graph[transition.From].Add(transition.To, transition.GetTransitionLength());

                    var nextTransitions = map.Waypoints
                        .SelectMany(w => w.Transitions)
                        .Where(t => t.From.Equals(transition.To));
                    
                    temp.AddRange(nextTransitions);
                }

                if (temp.Count == 0)
                {
                    break;
                }
                    
                transitions = temp.ToList();
                temp.Clear();
            }
            
            return graph;
        }

        private RouteInfo DijkstrasAlgorithm(Dictionary<IWaypoint, Dictionary<IWaypoint, float>> graph,
            IWaypoint endPoint)
        {
            var firstPair = graph.First();
            var parents = new Dictionary<IWaypoint, IWaypoint>();
            var costs = new Dictionary<IWaypoint, float>();
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

            var processed = new HashSet<IWaypoint>();
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

            var waypoints = new List<IWaypoint>();
            var length = 0f;
            var searchablePair = parents.FirstOrDefault(p => p.Key.Equals(endPoint));
            if (searchablePair.Equals(new KeyValuePair<IWaypoint, IWaypoint>()))
            {
                Debug.LogError($"{nameof(DijkstrasRouteNavigator)} >>> Can't find {(endPoint as MonoBehaviour)?.name}");
                return null;
            }
            
            waypoints.Add(searchablePair.Key);
            length += costs[searchablePair.Key];
            var lastParent = searchablePair.Value;
            while (true)
            {
                waypoints.Add(lastParent);
                if (lastParent.Equals(firstPair.Key))
                {
                    break;
                }
                
                length += costs[lastParent];
                lastParent = parents[lastParent];
            }

            waypoints.Reverse();
            
            var result = new RouteInfo(waypoints, length);
            return result;
        }

        private IWaypoint FindLowestCostNode(IDictionary<IWaypoint, float> costs, HashSet<IWaypoint> processed)
        {
            var lowesCost = float.MaxValue;
            IWaypoint waypoint = null;
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