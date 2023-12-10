using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public interface ITransition
    {
        public IWaypoint From { get; }
        public IWaypoint To { get; }
        Vector3 GetTransitionDirection();
    }
}