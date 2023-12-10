using Services.Job.Runtime;

namespace Core.ResourceExtraction.Executor.Finalizer
{
    public class ResourceExtractionFinalizer : IJobOperationExecutor
    {
        private readonly JobInfo m_JobInfo;

        public ResourceExtractionFinalizer(JobInfo jobInfo)
        {
            m_JobInfo = jobInfo;
        }

        public void Execute(IJobOperationInfo jobOperationInfo)
        {
            m_JobInfo.JobStatus = JobEnvironment.JobStatus.Done;
        }
    }
}