using System.Collections.Generic;
using System.Linq;
using Common;
using Core.Currency.Runtime;
using Core.Mine.Runtime.Waypoint;
using Core.ResourceExtraction.Executor.Deliverer;
using Core.ResourceExtraction.Executor.Finalizer;
using Core.ResourceExtraction.Executor.Gatherer;
using Core.ResourceExtraction.Executor.Starter;
using Services.Currency.Runtime.Rewards;
using Services.Job.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractor : IResourceExtractor, IRouteMoveListener
    {
        private readonly ResourceExtractorBody m_Body;
        private readonly IRouteMovement m_RouteMovement;
        private readonly Queue<IJob> m_Jobs;
        private readonly EmployeeInfo m_EmployeeInfo;
        
        private IJob m_ActiveJob;
        
        public ResourceExtractor(
            ResourceExtractorBody body,
            ISpeedService movementSpeedService,
            ICoroutineRunner coroutineRunner)
        {
            m_Body = body;
            m_RouteMovement = new ResourceExtractorMovement(this, movementSpeedService, coroutineRunner);
            
            m_Jobs = new Queue<IJob>();
            m_EmployeeInfo = new EmployeeInfo(EmployeeEnvironment.Unemployed);

        }
        
        public void EnqueueJob(IJob job)
        {
            m_Jobs.Enqueue(job);
            OnJobQueueChanged();
        }

        public void DequeueJob()
        {
            m_Jobs.Dequeue();
            OnJobQueueChanged();
        }
        
        public EmployeeInfo GetEmployeeInfo()
        {
            return m_EmployeeInfo;
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
            
            m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Unemployed;
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

        private void OnJobQueueChanged()
        {
            TryStartJob();
        }
        
        private bool TryStartJob()
        {
            if (m_Jobs.Count <= 0)
            {
                return false;
            }
            
            var job = m_Jobs.Peek();
            if (job.GetJobInfo().JobStatus == JobEnvironment.JobStatus.Todo)
            {
                m_ActiveJob = job;
                var starterInfo = new ResourceExtractionStarterInfo(this);
                
                m_ActiveJob.Execute(starterInfo);
                
                m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Working;
                return true;
            }
            
            return false;
        }
    }
}