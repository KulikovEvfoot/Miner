using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Services.Navigation.Tests
{
    internal class HighwayMock : IHighway
    {
        public IReadOnlyList<IPoint> Points { get; }

        public HighwayMock(IReadOnlyList<IPoint> points)
        {
            Points = points;
        }
    }
}