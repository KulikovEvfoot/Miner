using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using Services.Job.Runtime;

namespace Core.ResourceExtraction.Executor.Gatherer
{
    public class ResourceExtractionGatheringInfo : IJobOperationInfo
    {
        public IEnumerable<IReward> Rewards;
    }
}