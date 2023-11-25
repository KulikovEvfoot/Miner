using System;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Mine.Runtime.Transition
{
    [Serializable]
    public class Transition : TransitionBase
    {
        [SerializeField] private WaypointBase m_From;
        [SerializeField] private WaypointBase m_To;
        [SerializeField] private TransitionInfo m_TransitionInfo;

        public override IWaypoint From => m_From;
        public override IWaypoint To => m_To;
        
        public TransitionInfo TransitionInfo => m_TransitionInfo;

        public override float GetTransitionLength()
        {
            var p1 = From.Position;
            var p2 = To.Position;
            var lenght = Vector3.Distance(p1, p2);
            return lenght;
        }

        public override Vector3 GetTransitionDirection()
        {
            var heading = To.Position - From.Position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }
    }
}