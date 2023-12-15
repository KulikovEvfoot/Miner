using System.Collections.Generic;
using System.Linq;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class RouteMoveListenerMock : IRouteMoveListener
    {
        public Vector3 Position = default;
        public int PassedTransitionCount = 0;
        public int PassedRoutesCount = 0;

        public RouteMoveListenerMock()
        {
            Position = default;
            PassedTransitionCount = default;
            PassedRoutesCount = default;
        }

        public void Tick(RouteTickInfo routeTickInfo)
        {
            Position = routeTickInfo.Position;
        }

        public void NotifyOnTransitionComplete(IEnumerable<ITransition> passedTransitions)
        {
            PassedTransitionCount = passedTransitions.Count();
        }

        public void NotifyOnRouteComplete(IEnumerable<ITransition> route, int countOfCompletedLoops)
        {
            PassedRoutesCount = countOfCompletedLoops;
        }
    }
}