using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using Services.Job.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.ResourceExtraction.Executor.Starter
{
    public class ResourceExtractionStarter : IJobOperationExecutor
    {
        private readonly JobInfo m_JobInfo;
        private readonly IEnumerable<ITransition> m_Transitions;
        private readonly List<IReward> m_CollectedRewards;

        public ResourceExtractionStarter(
            JobInfo jobInfo, 
            IEnumerable<ITransition> transitions,
            List<IReward> collectedRewards)
        {
            m_JobInfo = jobInfo;
            m_Transitions = transitions;
            m_CollectedRewards = collectedRewards;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            if (jobOperationInfo is ResourceExtractionStarterInfo info)
            {
                if (!m_JobInfo.HasAssignee())
                {
                    return;
                }
            
                m_CollectedRewards.Clear();
                
                m_JobInfo.JobStatus = JobEnvironment.JobStatus.InProgress;

                info.ResourceExtractor.ResourceGathering(m_Transitions);
            }
        }
    }
}