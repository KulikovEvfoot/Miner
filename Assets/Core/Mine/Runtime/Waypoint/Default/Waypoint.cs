using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.Default
{
    public class Waypoint : WaypointBase
    {
        public override Vector3 Position => gameObject.transform.position;
    }
}