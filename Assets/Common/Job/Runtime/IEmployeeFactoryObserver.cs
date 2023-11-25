namespace Common.Job.Runtime
{
    public interface IEmployeeFactoryObserver
    {
        void NotifyOnEmployeeCreated(IEmployee employee);
    }
}