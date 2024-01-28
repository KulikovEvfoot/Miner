using Services.Navigation.Runtime.Scripts.Configs;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public class Transition : ITransition
    {
        public IPoint From { get; }
        public IPoint To { get; }
        
        public Transition(IPoint from, IPoint to)
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

        public float GetTransitionLength()
        {
            var result = Vector3.Distance(To.Position, From.Position);
            return result;
        }
    }
}