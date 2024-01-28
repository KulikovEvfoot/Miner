using System.Collections.Generic;
using Common;
using Core.Mine.Runtime.Point;
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

        public void ResourceGathering(IEnumerable<ITransition> transitions)
        {
            m_RouteMovement.EnRouteMove(transitions);
        }

        public void Tick(RouteTickInfo routeTickInfo)
        {
            m_Body.Move(routeTickInfo.Position);
        }

        public void NotifyOnTransitionComplete(IEnumerable<ITransition> passedTransitions)
        {

        }

        public void NotifyOnRouteComplete(IEnumerable<ITransition> route, int countOfCompletedLoops)
        {
            var resourcePoints = new List<IResourcePoint>();
            foreach (var transition in route)
            {
                if (transition.To is IResourcePoint resourcePoint)
                {
                    resourcePoints.Add(resourcePoint);
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