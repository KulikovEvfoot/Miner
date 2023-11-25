using Common.Job.Runtime;
using Common.Job.Runtime.ResourceExtraction;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.Currency.Runtime;
using Core.EventProducer.Runtime;
using UnityEngine;

namespace Core.Job.Runtime
{
    public class ResourceExtractionController : MonoBehaviour
    {
        [SerializeField] private WaypointBase m_WarehousePoint;
        
        private ResourceExtractionJobFactory m_ResourceExtractionJobFactory;
        
        public void Init(
            IMapRouter mapRouter, 
            RewardCollectorsController rewardCollectorsService, 
            EventProducer<IJobFactoryObserver> jobFactoryProducer,
            EventProducer<IJobInfoObserver> jobInfoProducer)
        {
            m_ResourceExtractionJobFactory = new ResourceExtractionJobFactory(
                mapRouter, 
                rewardCollectorsService, 
                m_WarehousePoint,
                jobInfoProducer);
            
            var jobs = m_ResourceExtractionJobFactory.Create();
            foreach (var job in jobs)
            {
                jobFactoryProducer.NotifyAll(obs => obs.NotifyOnJobCreated(job));
            }
        }
    }
}