using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.Base
{
    public class BasePoint : WaypointBase
    {
        public override Vector3 Position => gameObject.transform.position;
    }
}