using System;
using System.Collections.Generic;
using Core.Currency.Runtime;
using Core.Currency.Runtime.Gold;
using UnityEngine;

namespace Core.Mine.Runtime.Point.GoldResource
{
    [Serializable]
    public class GoldResourcePoint : IResourcePoint
    {
        [SerializeField] private int m_Id;
        [SerializeField] private int[] m_NeighborsID;
        [SerializeField] private Vector3 m_Position;
        [SerializeField] private GoldReward m_GoldReward;
        [SerializeField] private string m_ViewAddress; //can add dropdown menu to select view
        
        public int Id => m_Id;
        public int[] NeighborsID => m_NeighborsID;
        public Vector3 Position => m_Position;
        public IEnumerable<IResourceReward> ResourceRewards => new[] { m_GoldReward };
        public string ViewAddress => m_ViewAddress;

        public GoldResourcePoint(int id, int[] neighborsID, Vector3 position, GoldReward goldReward, string viewAddress)
        {
            m_Id = id;
            m_NeighborsID = neighborsID;
            m_Position = position;
            m_GoldReward = goldReward;
            m_ViewAddress = viewAddress;
        }

        public IEnumerable<IResourceReward> ExtractResources(int countOfLooping)
        {
            var result = new[] { new GoldReward(m_GoldReward.Value * countOfLooping) };
            return result;
        }
    }
}