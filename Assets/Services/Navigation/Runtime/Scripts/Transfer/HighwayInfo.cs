using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    internal struct HighwayInfo
    {
        public bool IsPassed { get; private set; }
        public RouteTravelInfo RouteTravelInfo { get; private set; } 
        public List<IHighway> Highways { get; }
        
        public HighwayInfo(RouteTravelInfo routeTravelInfo, List<IHighway> highways)
        {
            IsPassed = highways.Count > 0;
            RouteTravelInfo = routeTravelInfo;
            Highways = highways;
        }

        public void Combine(HighwayInfo highwayInfo)
        {
            IsPassed = true;
            RouteTravelInfo = highwayInfo.RouteTravelInfo;
            Highways.AddRange(highwayInfo.Highways);
        }
    }
}
