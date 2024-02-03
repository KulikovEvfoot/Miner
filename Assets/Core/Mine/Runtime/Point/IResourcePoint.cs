using System.Collections.Generic;
using Core.Currency.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.Point
{
    public interface IResourcePoint : IPoint, IHasView
    {
        IEnumerable<IResourceReward> ExtractResources(int countOfLooping);
    }
}