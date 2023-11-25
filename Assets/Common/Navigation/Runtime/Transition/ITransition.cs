using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Navigation.Runtime.Transition
{
    public interface ITransition
    {
        public IWaypoint From { get; }
        public IWaypoint To { get; }
        float GetTransitionLength();
        Vector3 GetTransitionDirection();
    }
}