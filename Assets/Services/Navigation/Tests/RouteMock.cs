using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Services.Navigation.Tests
{
    internal class RouteMock : IRoute
    {
        public IReadOnlyList<IHighway> Highways { get; }

        public RouteMock(IReadOnlyList<IHighway> highways)
        {
            Highways = highways;
        }
    }
}