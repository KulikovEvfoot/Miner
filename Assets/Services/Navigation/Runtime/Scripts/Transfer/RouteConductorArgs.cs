namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorArgs
    {
        public IRoute Route { get; }
        public float Speed { get; }
        public RouteTravelInfo RouteTravelInfo { get; }

        public RouteConductorArgs(IRoute route, float speed, RouteTravelInfo routeTravelInfo)
        {
            Route = route;
            Speed = speed;
            RouteTravelInfo = routeTravelInfo;
        }
    }
}