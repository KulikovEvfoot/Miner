using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public class MineMap
    {
        public IReadOnlyDictionary<int, IPoint> Points { get; }
        
        public IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> NavigationMap { get; }

        public MineMap(
            IReadOnlyDictionary<int, IPoint> points,
            IReadOnlyDictionary<IPoint, IReadOnlyList<IPoint>> navigationMap)
        {
            Points = points;
            NavigationMap = navigationMap;
        }
    }
}