using System;
using System.Collections.Generic;
using Common.Job.Runtime;
using Common.Moving.Runtime;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.Job.Runtime.ResourceExtraction;
using Core.Mine.Runtime.Waypoint;

namespace Core.Units.Runtime.Miner
{
    public class Miner : IMiner
    {
        private readonly MinerBody m_Body;
        private readonly IRouteNavigator m_RouteNavigator;
        private readonly IUnitMovement m_UnitMovement;
        private readonly Queue<IJob> m_Jobs;
        private readonly EmployeeInfo m_EmployeeInfo;
        
        private Action<string> m_OperationResult;

        public IUnitMovement UnitMovement => m_UnitMovement;
        
        public Miner(MinerBody body, IRouteNavigator routeNavigator, IUnitMovement unitMovement)
        {
            m_Body = body;
            m_RouteNavigator = routeNavigator;
            m_UnitMovement = unitMovement;
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
        
        void IResourceExtractor.MoveTo(IWaypoint waypoint, Action<string> operationResult)
        {
            m_OperationResult = operationResult;
            var routeInfo = m_RouteNavigator.BuildRoute(m_Body.GetWaypoint(), waypoint);
            m_UnitMovement.EnRouteMove(m_Body, routeInfo.Waypoints, ExtractorMovementResultConverter);
        }

        void IResourceExtractor.OnExtractingResource(IResourcePoint resourcePoint, Action<string> operationResult)
        {
            resourcePoint.ResourceExtracted();
            operationResult?.Invoke(ResourceExtractorEnvironment.StageDone);
        }

        void IResourceExtractor.FinalizeJob()
        {
            m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Unemployed;
            var job = m_Jobs.Peek();
            job.FinalizeJob();
        }
        
        private void ExtractorMovementResultConverter(string movementResult)
        {
            if (movementResult == UnitMovementEnvironment.CompleteKey)
            {
                m_OperationResult?.Invoke(ResourceExtractorEnvironment.StageDone);
            }
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
                m_EmployeeInfo.EmployeeStatus = EmployeeEnvironment.Working;
                return true;
            }
            
            return false;
        }
    }
}