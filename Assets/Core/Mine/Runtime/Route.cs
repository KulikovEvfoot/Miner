using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public class Route : IRoute
    {
        public IReadOnlyList<IHighway> Highways { get; }

        public Route(IReadOnlyList<IHighway> highways)
        {
            Highways = highways;
        }
    }
}