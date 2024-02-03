using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public static class PointUtils
    {
        public static Vector3 GetTransitionDirection(IPoint to, IPoint from)
        {
            var heading = to.Position - from.Position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public static float GetTransitionLength(IPoint to, IPoint from)
        {
            var result = Vector3.Distance(to.Position, from.Position);
            return result;
        }
    }
}