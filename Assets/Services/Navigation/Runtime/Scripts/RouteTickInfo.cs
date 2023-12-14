using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public struct RouteTickInfo
    {
        public Vector3 Position { get; }

        public RouteTickInfo(Vector3 position)
        {
            Position = position;
        }
    }
}