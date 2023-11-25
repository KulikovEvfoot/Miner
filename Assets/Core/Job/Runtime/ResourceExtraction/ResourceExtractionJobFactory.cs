using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Common.Wallet.Runtime.Rewards;
using Core.Currency.Runtime;
using Core.EventProducer.Runtime;
using Core.Mine.Runtime.Waypoint;

namespace Common.Job.Runtime.ResourceExtraction
{
    public class ResourceExtractionJobFactory
    {
        private readonly IWaypoint m_Waypoint;
        private readonly IMapRouter m_MapRouter;
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly IWaypoint m_Warehouse;
        private readonly EventProducer<IJobInfoObserver> m_JobInfoProducer;

        public ResourceExtractionJobFactory(
            IMapRouter mapRouter, 
            RewardCollectorsController rewardCollectorsService,
            IWaypoint warehouse,
            EventProducer<IJobInfoObserver> jobInfoProducer)
        {
            m_MapRouter = mapRouter;
            m_RewardCollectorsService = rewardCollectorsService;
            m_Warehouse = warehouse;
            m_JobInfoProducer = jobInfoProducer;
        }

        public IEnumerable<IJob> Create()
        {
            var jobs = new List<IJob>();
            var resourcePoints = m_MapRouter.FindAllWaypointsByType<IResourcePoint>();
            foreach (var resourcePoint in resourcePoints)
            {
                var rewards = resourcePoint.ResourceDeposits
                    .Select(resourceDeposit => resourceDeposit.Reward)
                    .ToList();
                
                IJob job = new ResourceExtractionJob(
                    resourcePoint as IWaypoint, 
                    m_Warehouse, 
                    m_RewardCollectorsService, 
                    rewards, 
                    m_JobInfoProducer);
                
                jobs.Add(job);
            }

            return jobs;
        }
    }
}