using Common;
using Core.Mine.Runtime;
using Core.ResourceExtraction;
using Services.Job.Runtime;
using UnityEngine;

namespace Core
{
    public class ResourceExtractionJobController : MonoBehaviour
    {
        [SerializeField] private RouteBase m_Route;
        
        private ResourceExtractionJobFactory m_ResourceExtractionJobFactory;
        private EventProducer<IJobFactoryObserver> m_JobFactoryProducer;

        public void Init(
            RewardCollectorsController rewardCollectorsService, 
            EventProducer<IJobFactoryObserver> jobFactoryProducer,
            EventProducer<IJobInfoObserver> jobInfoProducer)
        {
            m_ResourceExtractionJobFactory = new ResourceExtractionJobFactory(
                m_Route.Transitions,
                rewardCollectorsService, 
                jobInfoProducer);

            m_JobFactoryProducer = jobFactoryProducer;
        }

        public void CreateJob()
        {
            var jobs = m_ResourceExtractionJobFactory.Create();
            foreach (var job in jobs)
            {
                m_JobFactoryProducer.NotifyAll(obs => obs.NotifyOnJobCreated(job));
            }
        }
    }
}