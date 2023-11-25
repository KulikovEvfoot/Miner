using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Moving.Runtime
{
    public interface IMovableBody
    {
        Transform Transform { get; }
        
        IWaypoint GetWaypoint();
        void SetWaypoint(IWaypoint waypoint);
    }
}