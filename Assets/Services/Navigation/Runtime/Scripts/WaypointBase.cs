using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public abstract class WaypointBase : MonoBehaviour, IWaypoint
    {
        public abstract Vector3 Position { get; }
    }
}