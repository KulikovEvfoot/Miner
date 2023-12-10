namespace Services.Job.Runtime
{
    public interface IEmployeeFactoryObserver
    {
        void NotifyOnEmployeeCreated(IEmployee employee);
    }
}