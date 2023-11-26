using Common;

namespace Core.Units.Runtime.Miner
{
    public interface IMinerFactory
    {
        Result<IMiner> Create();
    }
}