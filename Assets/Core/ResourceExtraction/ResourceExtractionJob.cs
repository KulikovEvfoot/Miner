using System;
using System.Collections.Generic;
using Core.ResourceExtraction.Executor.Deliverer;
using Core.ResourceExtraction.Executor.Finalizer;
using Core.ResourceExtraction.Executor.Gatherer;
using Core.ResourceExtraction.Executor.Starter;
using Core.ResourceExtraction.ResourceExtractor;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public class ResourceExtractionJob : IResourceExtractionJob
    {
        private readonly IRoute m_Route;
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly Dictionary<Type, IJobOperationExecutor> m_JobProgressMap;
        
        private readonly ResourceExtractionStarter m_ResourceExtractionStarter;
        private readonly ResourceExtractionFinalizer m_ResourceExtractionFinalizer;
        private readonly ResourceExtractionGatherer m_ResourceExtractionGatherer;
        private readonly ResourceExtractionDeliverer m_ResourceExtractionDeliverer;

        private List<IReward> m_CollectedRewards;

        public ResourceExtractionJob(
            IRoute route,
            RewardCollectorsController rewardCollectorsService)
        {
            m_Route = route;
            m_RewardCollectorsService = rewardCollectorsService;

            m_CollectedRewards = new List<IReward>();
            
            m_JobProgressMap = new Dictionary<Type, IJobOperationExecutor>();
            
            m_ResourceExtractionStarter = new ResourceExtractionStarter(m_Route, m_CollectedRewards);
            m_JobProgressMap.Add(typeof(ResourceExtractionStarterInfo), m_ResourceExtractionStarter);
            
            m_ResourceExtractionFinalizer = new ResourceExtractionFinalizer();
            m_JobProgressMap.Add(typeof(ResourceExtractionFinalizerInfo), m_ResourceExtractionFinalizer);
            
            m_ResourceExtractionGatherer = new ResourceExtractionGatherer(m_CollectedRewards);
            m_JobProgressMap.Add(typeof(ResourceExtractionGatheringInfo), m_ResourceExtractionGatherer);
            
            m_ResourceExtractionDeliverer = new ResourceExtractionDeliverer(
                m_RewardCollectorsService, m_CollectedRewards);
            m_JobProgressMap.Add(typeof(ResourceExtractionDelivererInfo), m_ResourceExtractionDeliverer);
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