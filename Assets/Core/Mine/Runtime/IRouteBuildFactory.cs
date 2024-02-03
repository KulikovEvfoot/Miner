using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public interface IRouteBuildFactory
    {
        IReadOnlyList<IPoint> Create();
    }
}