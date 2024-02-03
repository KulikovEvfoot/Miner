using System.Collections.Generic;
using System.Linq;
using Core.Mine.Runtime.RouteBuildType;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime
{
    [CreateAssetMenu(fileName = "MineConfig", menuName = "Config/MineConfig", order = 0)]
    public class MineConfig : ScriptableObject, IMineConfig
    {
        [SerializeReference] public List<IPoint> Points;
        [SerializeReference] public IRouteBuildType RouteBuildType;

        public IRouteBuildType GetRouteBuildType() => RouteBuildType;

        public MineMap GetMineMap()
        {
            var points = GetPoints();
            var navigationMap = GetNavigationMap();
            var mineMap = new MineMap(points, navigationMap);
            return mineMap;
        }
        
        private IReadOnlyDictionary<int, IPoint> GetPoints()
        {
            return Points.ToDictionary(point => point.Id);
        }
        
        private IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> GetNavigationMap()
        {
            var navigationMap = new Dictionary<IPoint, IReadOnlyList<IPoint>>();

            foreach (var point in Points)
            {
                if (navigationMap.ContainsKey(point))
                {
                    continue;
                }

                var route = new Dictionary<int, IPoint>();
                foreach (var neighborID in point.NeighborsID)
                {
                    if (point.Id == neighborID)
                    {
                        Debug.LogError(
                            $"[{nameof(MineConfig)}]: Point can't have a neighbor with the same identity." +
                            $"Id = {point.Id}");

                        return null;
                    }

                    var neighbour = Points.FirstOrDefault(p => p.Id == neighborID);
                    if (neighbour == null)
                    {
                        Debug.LogError(
                            $"[{nameof(MineConfig)}]: Can't find neighbour. " +
                            $"Point id = {point.Id}, neighbor id = {neighborID}");

                        return null;
                    }

                    if (route.ContainsKey(neighbour.Id))
                    {
                        Debug.LogError(
                            $"[{nameof(MineConfig)}]: Duplicate id. " +
                            $"Point id = {point.Id}, neighbor id = {neighborID}");

                        return null;
                    }

                    route.Add(neighborID, neighbour);
                }

                navigationMap.Add(point, new List<IPoint>(route.Values));
            }

            return navigationMap;
        }
    }
}