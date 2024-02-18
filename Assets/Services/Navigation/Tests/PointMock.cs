using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public class PointMock : IPoint
    {
        public int Id { get; }
        public int[] NeighborsID { get; }
        public Vector3 Position { get; }

        public PointMock(int id, int[] neighborsID, Vector3 position)
        {
            Id = id;
            NeighborsID = neighborsID;
            Position = position;
        }
    }
}