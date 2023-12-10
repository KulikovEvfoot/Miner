namespace Services.Job.Runtime
{
    public interface IJobFactoryObserver
    {
        void NotifyOnJobCreated(IJob job);
    }
}