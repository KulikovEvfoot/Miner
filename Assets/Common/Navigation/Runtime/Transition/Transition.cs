using System;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Navigation.Runtime.Transition
{
    [Serializable]
    public class Transition : ITransition
    {
        [SerializeField] private WaypointBase m_From;
        [SerializeField] private WaypointBase m_To;

        public IWaypoint From => m_From;
        public IWaypoint To => m_To;
        
        public float GetTransitionLength()
        {
            var p1 = From.Position;
            var p2 = To.Position;
            var lenght = Vector3.Distance(p1, p2);
            return lenght;
        }

        public Vector3 GetTransitionDirection()
        {
            var heading = To.Position - From.Position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }
    }
}