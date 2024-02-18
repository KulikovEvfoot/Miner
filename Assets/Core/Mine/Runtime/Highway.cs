using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public class Highway : IHighway
    {
        public IReadOnlyList<IPoint> Points { get; }

        public Highway(IReadOnlyList<IPoint> points)
        {
            Points = points;
        }
    }
}