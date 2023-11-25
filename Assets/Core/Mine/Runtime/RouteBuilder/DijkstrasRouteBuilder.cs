using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;

namespace Core.NavigationSystem.Runtime
{
    public class DijkstrasRouteBuilder : IRouteBuilder
    {
        private readonly IMapRouter m_MapRouter;
        private Dictionary<IWaypoint, Dictionary<IWaypoint, float>> m_Map;

        public DijkstrasRouteBuilder(IMapRouter mapRouter)
        {
            m_MapRouter = mapRouter;
        }
        
        public void CreateGraph(IWaypoint startPoint)
        {
            m_Map = new Dictionary<IWaypoint, Dictionary<IWaypoint, float>>();

            foreach (var route in m_MapRouter.Routes)
            {
                var transitions = route.Transitions.Where(t => t.From.Equals(startPoint)).ToList();
                
                var temp = new List<ITransition>();
                while (true)
                {
                    foreach (var transition in transitions)
                    {
                        if (!m_Map.ContainsKey(transition.From))
                        {
                            m_Map.Add(transition.From, new Dictionary<IWaypoint, float>());
                        }

                        if (transition.To.Equals(startPoint))
                        {
                            continue;
                        }

                        if (m_Map[transition.From].ContainsKey(transition.To))
                        {
                            continue;
                        }
                        
                        m_Map[transition.From].Add(transition.To, transition.GetTransitionLength());

                        var col = route.Transitions.Where(t => t.From.Equals(transition.To));
                        temp.AddRange(col);
                    }

                    if (temp.Count == 0)
                    {
                        break;
                    }
                    
                    transitions = temp.ToList();
                    temp.Clear();
                }
            }
            
            // foreach (var route in m_MapRouter.Routes)
            // {
            //     foreach (var transition in route.Transitions)
            //     {
            //         if (!m_Map.ContainsKey(transition.From))
            //         {
            //             m_Map.Add(transition.From, new Dictionary<IWaypoint, float>());
            //         }
            //
            //         if (!m_Map.ContainsKey(transition.To))
            //         {
            //             m_Map.Add(transition.To, new Dictionary<IWaypoint, float>());
            //         }
            //         
            //         m_Map[transition.From].Add(transition.To, transition.GetTransitionLength());
            //     }
            // }
        }
        
        public RouteInfo BuildRoute(IWaypoint startPoint, IWaypoint endPoint)
        {
            CreateGraph(startPoint);
            var result = DijkstrasAlgorithm(endPoint);
            return result;
        }

        private RouteInfo DijkstrasAlgorithm(IWaypoint endPoint)
        {
            // var firstPair = graph.FirstOrDefault(g => g.Key.Equals(startPoint));
            var graph = m_Map;
            var firstPair = m_Map.First();
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
            IWaypoint IWaypoint = null;
            foreach (var node in costs)
            {
                var cost = node.Value;
                if (cost < lowesCost && !processed.Contains(node.Key))
                {
                    lowesCost = cost;
                    IWaypoint = node.Key;
                }
            }

            return IWaypoint;
        }
    }
}