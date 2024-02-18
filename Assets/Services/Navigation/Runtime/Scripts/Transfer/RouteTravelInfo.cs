using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteTravelInfo
    {
        public readonly int CurrentHighwayIndex;
        public readonly int CurrentPointIndex;
        public readonly Vector3 CurrentPosition;
        
        public float DeltaTime;

        public RouteTravelInfo(
            int currentHighwayIndex,
            int currentPointIndex,
            Vector3 currentPosition,
            float deltaTime)
        {
            CurrentHighwayIndex = currentHighwayIndex;
            CurrentPointIndex = currentPointIndex;
            CurrentPosition = currentPosition;
            DeltaTime = deltaTime;
        }

        public void UpdateDeltaTime(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}