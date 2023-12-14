using System;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    [Serializable]
    public class Transition : ITransition
    {
        [SerializeField] private WaypointBase m_From;
        [SerializeField] private WaypointBase m_To;

        public IWaypoint From => m_From;
        public IWaypoint To => m_To;

        public Vector3 GetTransitionDirection()
        {
            var heading = To.Position - From.Position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public float GetTransitionLenght()
        {
            var result = Vector3.Distance(To.Position, From.Position);
            return result;
        }
    }
}