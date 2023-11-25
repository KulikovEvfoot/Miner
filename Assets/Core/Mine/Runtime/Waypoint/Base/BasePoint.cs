using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.NavigationSystem.Runtime
{
    public class BasePoint : WaypointBase
    {
        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
    }
}