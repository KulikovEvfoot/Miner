using System;
using UnityEngine;

namespace Core.Mine.Runtime.Point.Base
{
    [Serializable]
    public class BasePoint : IBasePoint
    { 
        [SerializeField, HideInInspector] private string m_Name = nameof(BasePoint);
        [SerializeField] private int m_Id;
        [SerializeField] private int[] m_NeighborsID;
        [SerializeField] private Vector3 m_Position;

        public int Id => m_Id;
        public int[] NeighborsID => m_NeighborsID;
        public Vector3 Position => m_Position;

        public BasePoint(int id, int[] neighborsID, Vector3 position)
        {
            m_Id = id;
            m_NeighborsID = neighborsID;
            m_Position = position;
        }
    }
}