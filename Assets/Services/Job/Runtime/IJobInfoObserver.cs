namespace Services.Job.Runtime
{
    public interface IJobInfoObserver
    {
        public void NotifyOnJobInfoChanged(JobInfo jobInfo);
    }
}