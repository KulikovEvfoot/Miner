using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorResult
    {
        public IRoute Route { get; }
        public int PassedRoutesCount { get; }
        public IReadOnlyList<IHighway> PassedHighways { get; }
        public IReadOnlyList<IPoint> PassedPointsOnCurrentHighway { get; }
        public RouteTravelInfo RouteTravelInfo { get; }

        public RouteConductorResult(
            IRoute route,
            int passedRoutesCount, 
            IReadOnlyList<IHighway> passedHighways, 
            IReadOnlyList<IPoint> passedPointsOnCurrentHighway, 
            RouteTravelInfo routeTravelInfo)
        {
            Route = route;
            PassedRoutesCount = passedRoutesCount;
            PassedHighways = passedHighways;
            PassedPointsOnCurrentHighway = passedPointsOnCurrentHighway;
            RouteTravelInfo = routeTravelInfo;
        }
    }
}