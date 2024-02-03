using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorArgs
    {
        public IReadOnlyList<IPoint> Route;
        public int LastPassedPointIndex;
        public Vector3 CurrentPosition;
        public ISpeedService SpeedService;
        public float DeltaTime;
    }
}