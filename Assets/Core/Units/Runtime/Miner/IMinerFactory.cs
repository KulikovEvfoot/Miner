using Common;

namespace Core.Units.Runtime.Miner
{
    public interface IMinerFactory
    {
        Result<Miner> Create();
    }
}