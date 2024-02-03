using System;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    [Serializable]
    public class Point : IPoint
    {
        [SerializeField] private int m_Id;
        [SerializeField] private int[] m_NeighborsID;
        [SerializeField] private Vector3 m_Position;

        public int Id => m_Id;
        public int[] NeighborsID => m_NeighborsID;
        public Vector3 Position => m_Position;

        public Point(int id, int[] neighborsID, Vector3 position)
        {
            m_Id = id;
            m_NeighborsID = neighborsID;
            m_Position = position;
        }
    }
}