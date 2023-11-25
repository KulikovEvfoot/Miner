using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.Default
{
    public class Waypoint : WaypointBase
    {
        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
    }
}