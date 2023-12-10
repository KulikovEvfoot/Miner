namespace Services.Job.Runtime
{
    public interface IJob
    {
        JobInfo GetJobInfo();
        void Execute(IJobOperationInfo jobOperationInfo);
    }
}