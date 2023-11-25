using Common.Job.Runtime.Info;

namespace Common.Job.Runtime
{
    public interface IJobInfoObserver
    {
        public void NotifyOnJobInfoChanged(JobInfo jobInfo);
    }
}