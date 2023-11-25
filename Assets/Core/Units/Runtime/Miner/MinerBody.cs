using Common.Moving.Runtime;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Mine.Runtime
{
    public class MinerBody : MonoBehaviour, IMovableBody
    {
        private IWaypoint m_CurrentWaypoint;
        
        public Transform Transform => gameObject.transform;
        
        public IWaypoint GetWaypoint()
        {
            return m_CurrentWaypoint;
        }

        public void SetWaypoint(IWaypoint waypoint)
        {
            m_CurrentWaypoint = waypoint;
        }
    }
}