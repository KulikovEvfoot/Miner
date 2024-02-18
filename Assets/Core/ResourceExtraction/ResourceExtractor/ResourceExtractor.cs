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

        public void ResourceGathering(IRoute route)
        {
            m_RouteMovement.EnRouteMove(route);
        }
        
        public void Tick(RouteConductorResult routeTickInfo)
        {
            m_Body.Move(routeTickInfo.RouteTravelInfo.CurrentPosition);
            
            if (routeTickInfo.PassedRoutesCount > 0)
            {
                OnHighwaysComplete(routeTickInfo.Route.Highways, routeTickInfo.PassedRoutesCount);
            }

            if (routeTickInfo.PassedHighways.Count > 0)
            {
                OnHighwaysComplete(routeTickInfo.PassedHighways, 1);
            }
        }
        
        private void OnHighwaysComplete(IEnumerable<IHighway> highways, int countOfCompletedLoops)
        {
            var resourcePoints = new HashSet<IResourcePoint>();
            foreach (var highway in highways)
            {
                foreach (var point in highway.Points)
                {
                    if (point is IResourcePoint resourcePoint)
                    {
                        resourcePoints.Add(resourcePoint);
                    }
                }
            }
            
            if (resourcePoints.Count == 0)
            {
                return;
            }
        
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
            //TODO вынести в отдельный сервис все вычисления
            var rewards = new HashSet<IReward>();
            foreach (var resourcePoint in resourcePoints)
            {
                var resourceRewards = resourcePoint.ExtractResources(countOfLooping);
                foreach (var resourceReward in resourceRewards)
                {
                    rewards.Add(resourceReward);
                }
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