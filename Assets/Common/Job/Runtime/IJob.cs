namespace Common.Job.Runtime
{
    public interface IJob
    {
        JobInfo GetJobInfo();
        void StartJob();
        void FinalizeJob();
    }
}