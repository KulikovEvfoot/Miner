using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Navigation.Runtime.Transition
{
    public abstract class TransitionBase : MonoBehaviour, ITransition
    {
        public abstract IWaypoint From { get; }
        public abstract IWaypoint To { get; }
        public abstract float GetTransitionLength();
        public abstract Vector3 GetTransitionDirection();
    }
}