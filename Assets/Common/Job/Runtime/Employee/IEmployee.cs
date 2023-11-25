namespace Common.Job.Runtime.Employee
{
    public interface IEmployee
    {
        void EnqueueJob(IJob job);
        void DequeueJob();
        EmployeeStatus GetEmployeeStatus(); //todo заменить на info
    }
}