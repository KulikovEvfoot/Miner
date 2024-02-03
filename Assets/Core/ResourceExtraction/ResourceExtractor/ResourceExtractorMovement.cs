using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractorMovement : IRouteMovement
    {
        private readonly ICoroutineRunner m_CoroutineRunner;
        private readonly ISpeedService m_SpeedService;
        private readonly IRouteConductor m_RouteConductor;
        
        private Coroutine m_RouteRoutine;
        private readonly IRouteMoveListener m_RouteMoveListener;

        public ResourceExtractorMovement(
            IRouteMoveListener routeMoveListener, 
            ISpeedService speedService,
            ICoroutineRunner coroutineRunner,
            IRouteConductor routeConductor)
        {
            m_RouteMoveListener = routeMoveListener;
            m_SpeedService = speedService;
            m_CoroutineRunner = coroutineRunner;
            m_RouteConductor = routeConductor;
        }

        public void EnRouteMove(IEnumerable<IPoint> route)
        {
            m_RouteRoutine = m_CoroutineRunner.StartCoroutine(EnRouteMoveCoroutine(route.ToList()));
        }

        private IEnumerator EnRouteMoveCoroutine(IReadOnlyList<IPoint> route)
        {
            var previousFrameTime = Time.realtimeSinceStartup;

            //can set startIndex from save
            var startIndex = 0;
            var conductorResult = new RouteConductorResult(startIndex, route[startIndex].Position);
            
            while (true)
            {
                var deltaTime = Time.realtimeSinceStartup - previousFrameTime;

                conductorResult = m_RouteConductor.Conduct(new RouteConductorArgs
                {
                    Route = route,
                    LastPassedPointIndex = conductorResult.LastPassedIndex,
                    CurrentPosition = conductorResult.CurrentPosition,
                    SpeedService = m_SpeedService,
                    DeltaTime = deltaTime,
                });
                
                m_RouteMoveListener.Tick(conductorResult);
                previousFrameTime = Time.realtimeSinceStartup;
                yield return null;
            }
        }
    }
}