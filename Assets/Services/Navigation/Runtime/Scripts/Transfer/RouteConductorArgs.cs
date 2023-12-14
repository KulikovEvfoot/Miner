using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorArgs
    {
        public IList<ITransition> Route;
        public int TransitionIndex;
        public Vector3 Position;
        public ISpeedService SpeedService;
        public float DeltaTime;
        public IRouteMoveListener RouteMoveListener;
    }
}