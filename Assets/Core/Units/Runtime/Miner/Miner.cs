using System;
using System.Collections.Generic;
using Common.Job.Runtime;
using Common.Job.Runtime.Employee;
using Common.Job.Runtime.ResourceExtraction;
using Common.Moving.Runtime;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;

namespace Core.Mine.Runtime
{
    public class Miner : IResourceExtractor
    {
        private readonly MinerBody m_Body;
        private readonly IRouteBuilder m_RouteBuilder;
        private readonly IUnitMovement m_UnitMovement;
        private readonly Queue<IJob> m_Jobs;

        private Action<string> m_OperationResult;
        
        public Miner(MinerBody body, IRouteBuilder routeBuilder, IUnitMovement unitMovement)
        {
            m_Body = body;
            m_RouteBuilder = routeBuilder;
            m_UnitMovement = unitMovement;
            m_Jobs = new Queue<IJob>();
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
        
        public EmployeeStatus GetEmployeeStatus()
        {
            if (m_Jobs.Count == 0)
            {
                return EmployeeStatus.Unemployed;
            }

            return EmployeeStatus.Working;
        }
        
        private void MovementResultConverter(string movementResult)
        {
            if (movementResult == m_UnitMovement.CompleteKey)
            {
                m_OperationResult?.Invoke(ResourceExtractorEnvironment.StageDone);
            }
        }

        public void MoveTo(IWaypoint waypoint, Action<string> operationResult)
        {
            m_OperationResult = operationResult;
            var routeInfo = m_RouteBuilder.BuildRoute(m_Body.GetWaypoint(), waypoint);
            m_UnitMovement.EnRouteMove(m_Body, routeInfo.Waypoints, MovementResultConverter);
        }

        public void OnExtractingResource(Action<string> operationResult)
        {
            operationResult?.Invoke(ResourceExtractorEnvironment.StageDone);
        }

        public void FinalizeJob()
        {
            var job = m_Jobs.Peek();
            job.FinalizeJob();
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
                job.StartJob();
                return true;
            }
            
            return false;
        }
    }
}