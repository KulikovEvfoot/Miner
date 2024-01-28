using System.Collections.Generic;
using Core.Currency.Runtime;
using Services.Navigation.Runtime.Scripts.Configs;

namespace Core.Mine.Runtime.Point
{
    public interface IResourcePoint : IPoint, IHasView
    {
        IEnumerable<IResourceReward> ExtractResources(int countOfLooping);
    }
}