using UnityEngine;

namespace Common.Navigation.Runtime.Waypoint
{
    public abstract class WaypointBase : MonoBehaviour, IWaypoint
    {
        public abstract Transform Transform { get; }
        public abstract Vector3 Position { get; }
    }
}