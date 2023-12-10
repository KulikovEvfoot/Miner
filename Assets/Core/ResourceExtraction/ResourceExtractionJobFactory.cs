using System.Collections.Generic;
using Common;
using Services.Job.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction
{
    public class ResourceExtractionJobFactory
    {
        private readonly RewardCollectorsController m_RewardCollectorsService;
        private readonly EventProducer<IJobInfoObserver> m_JobInfoProducer;
        private readonly IEnumerable<ITransition> m_Transitions;

        public ResourceExtractionJobFactory(
            IEnumerable<ITransition> transitions,
            RewardCollectorsController rewardCollectorsService,
            EventProducer<IJobInfoObserver> jobInfoProducer)
        {
            m_Transitions = transitions;
            m_RewardCollectorsService = rewardCollectorsService;
            m_JobInfoProducer = jobInfoProducer;
        }

        public IEnumerable<IJob> Create()
        {
            var jobs = new List<IJob>();

            IJob job = new ResourceExtractionJob(
                m_Transitions,
                m_RewardCollectorsService, 
                m_JobInfoProducer);
                
            jobs.Add(job);

            return jobs;
        }
    }
}