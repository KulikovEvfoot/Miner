using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.ByPathfinding
{
    public class ByPathfindingBuildType : IRouteBuildType
    {
        [SerializeField] private PathfindingAlgorithmType m_PathfindingAlgorithmType;
        [SerializeField] private bool m_ReturnToBaseAfterResourcePoint;
        
        public PathfindingAlgorithmType PathfindingAlgorithmType => m_PathfindingAlgorithmType;

        public bool ReturnToBaseAfterResourcePoint => m_ReturnToBaseAfterResourcePoint;
    }

    public enum PathfindingAlgorithmType
    {
        Dijkstras = 10,
        
    }
}