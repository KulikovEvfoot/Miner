using System.Collections.Generic;
using Core.Currency.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.Waypoint
{
    public interface IResourcePoint : IWaypoint
    {
        IEnumerable<IResourceReward> ExtractResources(int countOfLooping);
    }
}