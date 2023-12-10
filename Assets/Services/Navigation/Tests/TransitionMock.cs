using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class TransitionMock : ITransition
    {
        public IWaypoint From { get; }
        public IWaypoint To { get; }
        
        public TransitionMock(IWaypoint from, IWaypoint to)
        {
            From = from;
            To = to;
        }

        public Vector3 GetTransitionDirection()
        {
            var heading = To.Position - From.Position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }
    }
}