using Common;

namespace Core.Mine.Runtime
{
    public interface IMinerFactory
    {
        Result<Miner> Create();
    }
}