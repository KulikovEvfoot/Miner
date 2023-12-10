namespace Services.Job.Runtime
{
    public interface IJobOperationExecutor
    {
        void Execute(IJobOperationInfo jobOperationInfo);
    }
}