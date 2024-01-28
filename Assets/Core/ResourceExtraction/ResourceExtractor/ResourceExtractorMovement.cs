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

        public void EnRouteMove(IEnumerable<ITransition> transitions)
        {
            m_RouteRoutine = m_CoroutineRunner.StartCoroutine(EnRouteMoveCoroutine(transitions.ToList()));
        }

        private IEnumerator EnRouteMoveCoroutine(IList<ITransition> transitions)
        {
            var previousFrameTime = Time.realtimeSinceStartup;
            var conductorResult = new RouteConductorResult(transitions[0].From.Position);
            
            while (true)
            {
                var deltaTime = Time.realtimeSinceStartup - previousFrameTime;

                conductorResult = m_RouteConductor.Conduct(new RouteConductorArgs
                {
                    Route = transitions,
                    TransitionIndex = conductorResult.Index,
                    Position = conductorResult.Position,
                    SpeedService = m_SpeedService,
                    DeltaTime = deltaTime,
                    RouteMoveListener = m_RouteMoveListener
                });
                
                previousFrameTime = Time.realtimeSinceStartup;
                yield return null;
            }
        }
    }
}