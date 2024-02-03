using System.Collections.Generic;
using System.Linq;
using Common;
using Core.Mine.Runtime.Point;
using Core.Mine.Runtime.Point.Base;
using Core.ResourceExtraction.Executor.Deliverer;
using Core.ResourceExtraction.Executor.Finalizer;
using Core.ResourceExtraction.Executor.Gatherer;
using Core.ResourceExtraction.Executor.Starter;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractor : IResourceExtractor, IRouteMoveListener
    {
        private readonly ResourceExtractorBody m_Body;
        private readonly IRouteMovement m_RouteMovement;
        
        private IResourceExtractionJob m_ActiveJob;

        private List<IPoint> m_ActiveRoute = new List<IPoint>();
        private List<IPoint> m_PassedPointsCache = new List<IPoint>();

        public ResourceExtractor(
            ResourceExtractorBody body,
            ISpeedService movementSpeedService,
            ICoroutineRunner coroutineRunner,
            IRouteConductor routeConductor)
        {
            m_Body = body;
            m_RouteMovement = new ResourceExtractorMovement(
                this,
                movementSpeedService, 
                coroutineRunner, 
                routeConductor);
        }

        public void StartJob(IResourceExtractionJob job)
        {
            m_ActiveJob = job;
            m_ActiveJob.Execute(new ResourceExtractionStarterInfo(this));
        }

        public void ResourceGathering(IEnumerable<IPoint> route)
        {
            m_RouteMovement.EnRouteMove(route);
            m_ActiveRoute = route.ToList();
        }
        
        public void Tick(RouteConductorResult routeTickInfo)
        {
            m_Body.Move(routeTickInfo.CurrentPosition);
            foreach (var passedPoint in routeTickInfo.PassedPoints)
            {
                m_PassedPointsCache.Add(passedPoint);
                if (passedPoint is BasePoint)
                {
                    OnRouteComplete(m_PassedPointsCache, 1);
                }
            }

            if (routeTickInfo.PassedRoutesCount > 0)
            {
                if (m_PassedPointsCache.Count == 0)
                {
                    m_PassedPointsCache = m_ActiveRoute;
                }
                
                OnRouteComplete(m_PassedPointsCache, routeTickInfo.PassedRoutesCount);
            }
        }
        
        private void OnRouteComplete(IEnumerable<IPoint> route, int countOfCompletedLoops)
        {
            var resourcePoints = new List<IResourcePoint>();
            foreach (var point in route)
            {
                if (point is IResourcePoint resourcePoint)
                {
                    resourcePoints.Add(resourcePoint);
                }
            }
            
            if (resourcePoints.Count == 0)
            {
                return;
            }

            m_PassedPointsCache.Clear();
            ExtractResources(resourcePoints, countOfCompletedLoops);
            DeliverResource();
        }
        
        public void FinalizeJob()
        {
            var finalizerInfo = new ResourceExtractionFinalizerInfo();
            m_ActiveJob.Execute(finalizerInfo);
        }
        
        private void ExtractResources(IEnumerable<IResourcePoint> resourcePoints, int countOfLooping)
        {
            var rewards = new List<IReward>();
            foreach (var resourcePoint in resourcePoints)
            {
                var resourceRewards = resourcePoint.ExtractResources(countOfLooping);
                rewards.AddRange(resourceRewards);
            }
            
            var gatheringInfo = new ResourceExtractionGatheringInfo
            {
                Rewards = rewards
            };
                
            m_ActiveJob.Execute(gatheringInfo);
        }

        private void DeliverResource()
        {
            var delivererInfo = new ResourceExtractionDelivererInfo();
            m_ActiveJob.Execute(delivererInfo);
        }
    }
}