using System;
using System.Collections.Generic;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime.Waypoint;

namespace Common.Moving.Runtime
{
    public interface IUnitMovement
    {
        IMovementSpeedService MovementSpeedService { get; }
        void EnRouteMove(IMovableBody movableBody, List<IWaypoint> route, Action<string> operationResult = null);
    }
}