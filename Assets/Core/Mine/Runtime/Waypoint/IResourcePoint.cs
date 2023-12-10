using System.Collections.Generic;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.Waypoint
{
    public interface IResourcePoint : IWaypoint
    {
        IEnumerable<IReward> Rewards { get; }
    }
}