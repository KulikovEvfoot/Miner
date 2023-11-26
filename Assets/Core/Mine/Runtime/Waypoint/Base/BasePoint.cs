using System.Collections.Generic;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.Base
{
    public class BasePoint : WaypointBase
    {
        [SerializeField] private Transition[] m_Transitions;
        
        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
        public override IEnumerable<ITransition> Transitions => m_Transitions;
    }
}