using System.Collections.Generic;
using Common.Navigation.Runtime.Waypoint;
using Core.Job.Runtime;

namespace Core.Mine.Runtime.Waypoint
{
    public interface IResourcePoint : IWaypoint
    {
        void ResourceExtracted();
        IEnumerable<IResourceDeposit> ResourceDeposits { get; }
    }
}