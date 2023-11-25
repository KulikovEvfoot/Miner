using Common.EventProducer;
using Common.EventProducer.Runtime;
using Common.Job;
using Common.Job.Runtime;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.Job.Runtime.ResourceExtraction;
using UnityEngine;

namespace Core
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