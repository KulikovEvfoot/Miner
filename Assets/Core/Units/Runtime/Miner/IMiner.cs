using Common.Moving.Runtime;
using Core.Job.Runtime.ResourceExtraction;

namespace Core.Units.Runtime.Miner
{
    public interface IMiner : IResourceExtractor
    {
        IUnitMovement UnitMovement { get; }
    }
}