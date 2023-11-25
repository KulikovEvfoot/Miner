using System.Collections.Generic;
using Core.Job.Runtime;

namespace Core.Mine.Runtime.Waypoint
{
    public interface IResourcePoint
    {
        IList<IResourceDeposit> ResourceDeposits { get; }
    }
}