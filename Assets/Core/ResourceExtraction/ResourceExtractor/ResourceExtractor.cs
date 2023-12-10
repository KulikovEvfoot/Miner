using System.Collections.Generic;
using Core.Mine.Runtime.Waypoint;
using Core.ResourceExtraction.Executor.Deliverer;
using Core.ResourceExtraction.Executor.Finalizer;
using Core.ResourceExtraction.Executor.Gatherer;
using Core.ResourceExtraction.Executor.Starter;
using Services.Job.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractor : IResourceExtractor
    {
        private readonly ResourceExtractorBody m_Body;
        private readonly IRouteMovement m_RouteMovement;
        private readonly Queue<IJob> m_Jobs;
        private readonly EmployeeInfo m_EmployeeInfo;
        
        private IJob m_ActiveJob;
        
        public ResourceExtractor(ResourceExtractorBody body, IRouteMovement routeMovement)
        {
            m_Body = body;
            m_RouteMovement = routeMovement;
            
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
        
        public void ResourceGathering(IEnumerable<ITransition> transitions, float time)
        {
            m_RouteMovement.EnRouteMove(
                transitions, 
                this,
                new TransferInfo(m_Body.Transform.position, time));
        }

        public void NotifyOnTransferComplete(ITransition transition)
        {
            var waypoint = transition.To;
            if (waypoint is IResourcePoint resourcePoint)
            {
                var gatheringInfo = new ResourceExtractionGatheringInfo
                {
                    Rewards = resourcePoint.Rewards
                };
                
                m_ActiveJob.Execute(gatheringInfo);
            }
        }

        public void NotifyOnRouteComplete(float time)
        {
            var delivererInfo = new ResourceExtractionDelivererInfo(this, time);
            m_ActiveJob.Execute(delivererInfo);
            
        }

        public void FinalizeJob()
        {
            var finalizerInfo = new ResourceExtractionFinalizerInfo();
            m_ActiveJob.Execute(finalizerInfo);
            
            m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Unemployed;
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
                var starterInfo = new ResourceExtractionStarterInfo(this, 0);
                
                m_ActiveJob.Execute(starterInfo);
                
                m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Working;
                return true;
            }
            
            return false;
        }
    }
}