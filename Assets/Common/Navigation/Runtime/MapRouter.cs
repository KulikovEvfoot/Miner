using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Navigation.Runtime
{
    public class MapRouter : MonoBehaviour, IMapRouter
    {
        [SerializeField] public List<Route> m_Routes;
        
        public List<Route> Routes => m_Routes;

        public List<T> FindAllWaypointsByType<T>()
        {
            var result = new List<T>();
            foreach (var waypoint in m_Routes.SelectMany(route => route.Waypoints))
            {
                if (waypoint is T point)
                {
                    result.Add(point);
                }
            }
            
            return result;
        }
        
        //сделать механизм объединения маршрутов
    }
}