using System.Collections.Generic;
using Common.Currency.Runtime.Rewards;
using Common.EventProducer;
using Common.EventProducer.Runtime;
using Common.Job;
using Common.Job.Runtime;
using Common.Navigation.Runtime.Waypoint;

namespace Core.Job.Runtime.ResourceExtraction
{
    public class ResourceExtractionJob : IJob
    {
        private const string MoveToResource = "move_to_resource";
        private const string ExtractResource = "extract_resource";
        private const string TransferResourceToBase = "transfer_resource_to_base";
        private const string FinalOperation = "final";
        
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly List<IReward> m_Rewards;
        private readonly JobInfo m_JobInfo;
        private readonly IWaypoint m_ResourcePoint;
        private readonly IWaypoint m_BasePoint;
        
        private JobOperationController m_JobOperationController;
        private IResourceExtractor m_ResourceExtractor;

        public ResourceExtractionJob(
            IWaypoint resourcePoint, 
            IWaypoint basePoint, 
            RewardCollectorsController rewardCollectorsService,
            List<IReward> rewards,
            EventProducer<IJobInfoObserver> eventProducer)
        {
            m_ResourcePoint = resourcePoint;
            m_BasePoint = basePoint;
            m_RewardCollectorsService = rewardCollectorsService;
            m_Rewards = rewards;
            m_JobInfo = new JobInfo(this, typeof(IResourceExtractor), eventProducer);
            CreateJobOperationController();
        }

        private void CreateJobOperationController()
        {
            var jobOperations = new Dictionary<string, JobOperation>();

            jobOperations.Add(MoveToResource, new JobOperation(
                MoveToResourceCall, 
                new Dictionary<string, string>
                {
                    {ResourceExtractorEnvironment.StageDone, ExtractResource}
                })
            );

            jobOperations.Add(ExtractResource, new JobOperation(
                ExtractResourceCall, 
                new Dictionary<string, string>
                {
                    {ResourceExtractorEnvironment.StageDone, TransferResourceToBase}
                })
            );
            
            jobOperations.Add(TransferResourceToBase, new JobOperation(
                TransferResourceToBaseCall, 
                new Dictionary<string, string>
                {
                    {ResourceExtractorEnvironment.StageDone, FinalOperation}
                })
            );
            
            jobOperations.Add(FinalOperation, new JobOperation(
                OnResourceDelivered, 
                new Dictionary<string, string>
                {
                    {ResourceExtractorEnvironment.StageDone, JobOperation.Done}
                })
            );
            
            m_JobOperationController = new JobOperationController(jobOperations);
        }

        public JobInfo GetJobInfo()
        {
            return m_JobInfo;
        }

        public void StartJob()
        {
            if (!m_JobInfo.HasAssignee())
            {
                return;
            }
            
            m_ResourceExtractor = m_JobInfo.Assignee as IResourceExtractor;
            m_JobInfo.JobStatus = JobEnvironment.JobStatus.InProgress;
            
            m_JobOperationController.InvokeOperation(MoveToResource);
        }

        public void FinalizeJob()
        {
            m_JobInfo.JobStatus = JobEnvironment.JobStatus.Done;
        }

        private void MoveToResourceCall()
        {
            m_ResourceExtractor.MoveTo(m_ResourcePoint, OnOperationComplete);
        }

        private void ExtractResourceCall()
        {
            m_ResourceExtractor.OnExtractingResource(OnOperationComplete);
        }

        private void TransferResourceToBaseCall()
        {
            m_ResourceExtractor.MoveTo(m_BasePoint, OnOperationComplete);
        }

        private void OnResourceDelivered()
        {
            foreach (var reward in m_Rewards)
            {
                m_RewardCollectorsService.CollectReward(reward);
            }

            OnOperationComplete(ResourceExtractorEnvironment.StageDone);
            m_ResourceExtractor.FinalizeJob();
        }
        
        private void OnOperationComplete(string result)
        {
            m_JobOperationController.OnOperationComplete(result);
        }
    }
}