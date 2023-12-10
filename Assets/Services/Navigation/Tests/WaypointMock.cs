using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class WaypointMock : IWaypoint
    {
        public Transform Transform { get; }
        public Vector3 Position { get; }

        public WaypointMock(Transform transform, Vector3 position)
        {
            Transform = transform;
            Position = position;
        }
    }
}