using System;
using System.Collections.Generic;
using Core.Mine.Runtime.Point.Base;
using Services.Navigation.Runtime.Scripts.Configs;
using UnityEngine;

namespace Core.Mine.Runtime
{
    [Serializable]
    public class MineConfig
    {
        [SerializeReference] private List<IPoint> m_Points = new List<IPoint>()
            // {
            //     new BasePoint(0, new []{1}, Vector3.zero),
            //     new GoldResourcePoint(1, new []{0}, new Vector3(0,1,0), new GoldReward(3), "GoldResourcePoint")
            // };
            ;
        [SerializeField] private MinePassageType m_MinePassageType;
        [SerializeField] private RouteBuildType m_RouteBuildType;
        
        public IReadOnlyList<IPoint> Points => m_Points;

        public void AddBasePoint()
        {
            m_Points.Add(new BasePoint(0, new []{1}, new Vector3()));
        }
        
        // [ContextMenu("ExFillPoints")]
        // private void ExFillPoints()
        // {
        //     m_Points = new List<IPoint>()
        //     {
        //         new BasePoint(0, new []{1}, Vector3.zero),
        //         new GoldResourcePoint(1, new []{0}, new Vector3(0,1,0), new GoldReward(3), "GoldResourcePoint")
        //     };
        // }
    }

    public enum MinePassageType
    {
        AllMinesInTurn = 0,
        ReturnToBaseAfterEachMine = 1,
    }

    public enum RouteBuildType
    {
        ByConfigIndexes = 0,
        ByPathfindingAlgorithm = 1,
    }
}