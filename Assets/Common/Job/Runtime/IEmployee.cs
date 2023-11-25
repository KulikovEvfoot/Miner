namespace Common.Job.Runtime
{
    public interface IEmployee
    {
        void EnqueueJob(IJob job);
        void DequeueJob();
        EmployeeInfo GetEmployeeInfo();
    }
}