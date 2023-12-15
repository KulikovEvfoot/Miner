using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class WaypointMock : IWaypoint
    {
        public Vector3 Position { get; }

        public WaypointMock(Vector3 position)
        {
            Position = position;
        }
    }
}