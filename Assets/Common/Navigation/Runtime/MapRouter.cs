using System.Collections.Generic;
using UnityEngine;

namespace Common.Navigation.Runtime
{
    public class MapRouter : MonoBehaviour, IMapRouter
    {
        [SerializeField] public Route m_Route;
        
        public Route Map => m_Route;

        public List<T> FindAllWaypointsByType<T>()
        {
            var result = new List<T>();
            foreach (var waypoint in m_Route.Waypoints)
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