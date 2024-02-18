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

        public void EnRouteMove(IRoute route)
        {
            m_RouteRoutine = m_CoroutineRunner.StartCoroutine(EnRouteMoveCoroutine(route));
        }

        private IEnumerator EnRouteMoveCoroutine(IRoute route)
        {
            var previousFrameTime = Time.realtimeSinceStartup;

            //can set startIndex from save
            var routeTravelInfo = new RouteTravelInfo(
                0,
                0,
                route.Highways[0].Points[0].Position,
                previousFrameTime);
            
            while (true)
            {
                var deltaTime = Time.realtimeSinceStartup - previousFrameTime;
                routeTravelInfo.UpdateDeltaTime(deltaTime);
                
                var conductorResult = m_RouteConductor.Conduct(new RouteConductorArgs(
                    route,
                    m_SpeedService.Speed, 
                    routeTravelInfo));

                routeTravelInfo = conductorResult.RouteTravelInfo;
                
                m_RouteMoveListener.Tick(conductorResult);
                previousFrameTime = Time.realtimeSinceStartup;
                yield return null;
            }
        }
    }
}