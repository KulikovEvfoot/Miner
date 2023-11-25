using System;
using System.Collections.Generic;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Navigation.Runtime
{
    public class Route : MonoBehaviour
    {
        [SerializeField] private WaypointBase[] m_WaypointBases;
        [SerializeField] private TransitionBase[] m_Transitions;
        [SerializeField] private Color m_DrawingColor = Color.white;
        
        private Dictionary<IWaypoint, List<IWaypoint>> m_WaypointsByType;

        public IList<IWaypoint> Waypoints => m_WaypointBases;
        public IList<ITransition> Transitions => m_Transitions;
        
        private void OnDrawGizmos()
        {
            if (m_Transitions == null)
            {
                return;
            }
        
            foreach (var transition in m_Transitions)
            {
                if (transition == null || transition.From == null || transition.To == null)
                {
                    return;
                }
                
                var direction = transition.GetTransitionDirection();
                float arrowHeadLength = 0.35f;
                float arrowHeadAngle = 25f;
                var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
                var left  = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
                var up    = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;
                var down  = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;
                var end   = transition.To.Position;
                
                var oldColor = Gizmos.color;
                Gizmos.color = m_DrawingColor;
                Gizmos.DrawRay(end, right);
                Gizmos.DrawRay(end, left);
                Gizmos.DrawRay(end, up);
                Gizmos.DrawRay(end, down);
                Gizmos.DrawLine(transition.From.Position, transition.To.Position);
                Gizmos.color = oldColor;
            }
        }
    }
}