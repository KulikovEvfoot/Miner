using System;
using System.Collections.Generic;
using Common;
using Core.ResourceExtraction.Executor.Deliverer;
using Core.ResourceExtraction.Executor.Finalizer;
using Core.ResourceExtraction.Executor.Gatherer;
using Core.ResourceExtraction.Executor.Starter;
using Services.Currency.Runtime.Rewards;
using Services.Job.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public class ResourceExtractionJob : IJob
    {
        private readonly IEnumerable<ITransition> m_Transitions;
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly Dictionary<Type, IJobOperationExecutor> m_JobProgressMap;
        private readonly JobInfo m_JobInfo;
        
        private readonly ResourceExtractionStarter m_ResourceExtractionStarter;
        private readonly ResourceExtractionFinalizer m_ResourceExtractionFinalizer;
        private readonly ResourceExtractionGatherer m_ResourceExtractionGatherer;
        private readonly ResourceExtractionDeliverer m_ResourceExtractionDeliverer;

        private List<IReward> m_CollectedRewards;

        public ResourceExtractionJob(
            IEnumerable<ITransition> transitions,
            RewardCollectorsController rewardCollectorsService,
            EventProducer<IJobInfoObserver> eventProducer)
        {
            m_Transitions = transitions;
            m_RewardCollectorsService = rewardCollectorsService;
            m_JobInfo = new JobInfo(this, typeof(IResourceExtractor), eventProducer);

            m_CollectedRewards = new List<IReward>();
            
            m_JobProgressMap = new Dictionary<Type, IJobOperationExecutor>();
            
            m_ResourceExtractionStarter = new ResourceExtractionStarter(m_JobInfo, m_Transitions, m_CollectedRewards);
            m_JobProgressMap.Add(typeof(ResourceExtractionStarterInfo), m_ResourceExtractionStarter);
            
            m_ResourceExtractionFinalizer = new ResourceExtractionFinalizer(m_JobInfo);
            m_JobProgressMap.Add(typeof(ResourceExtractionFinalizerInfo), m_ResourceExtractionFinalizer);
            
            m_ResourceExtractionGatherer = new ResourceExtractionGatherer(m_CollectedRewards);
            m_JobProgressMap.Add(typeof(ResourceExtractionGatheringInfo), m_ResourceExtractionGatherer);
            
            m_ResourceExtractionDeliverer = new ResourceExtractionDeliverer(
                m_RewardCollectorsService, m_CollectedRewards, this);
            m_JobProgressMap.Add(typeof(ResourceExtractionDelivererInfo), m_ResourceExtractionDeliverer);
        }
        
        public JobInfo GetJobInfo()
        {
            return m_JobInfo;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            var type = jobOperationInfo.GetType();
            if (m_JobProgressMap.TryGetValue(type, out var executor))
            {
                executor.Execute(jobOperationInfo);
            }
        }
    }
}