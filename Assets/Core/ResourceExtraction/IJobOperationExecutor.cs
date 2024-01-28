namespace Core.ResourceExtraction
{
    public interface IJobOperationExecutor
    {
        void Execute(IJobOperationInfo jobOperationInfo);
    }
}