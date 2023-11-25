using System;
using System.Collections.Generic;
using Common.Navigation.Runtime.Waypoint;

namespace Common.Moving.Runtime
{
    public interface IUnitMovement
    {
        void EnRouteMove(IMovableBody movableBody, List<IWaypoint> route, Action<string> operationResult = null);
    }
}