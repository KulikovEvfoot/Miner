using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;

namespace Core.ResourceExtraction.Executor.Gatherer
{
    public class ResourceExtractionGatheringInfo : IJobOperationInfo
    {
        public IEnumerable<IReward> Rewards;
    }
}