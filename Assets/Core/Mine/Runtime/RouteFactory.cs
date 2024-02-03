using System;
using System.Collections.Generic;
using Core.Mine.Runtime.RouteBuildType;
using Core.Mine.Runtime.RouteBuildType.ByPathfinding;
using Core.Mine.Runtime.RouteBuildType.Defined;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public class RouteFactory : IRouteBuildFactory
    {
        private readonly Dictionary<Type, IRouteBuildStrategy> m_BuildTypes = new()
        {
            [typeof(DefinedRouteBuildType)] = new DefinedRouteBuildStrategy(),
            [typeof(ByPathfindingBuildType)] =  new PathFindingBuildStrategy(),
        };
        
        private readonly IMineConfig m_MineConfig;

        public RouteFactory(IMineConfig mineConfig)
        {
            m_MineConfig = mineConfig;
        }

        public IReadOnlyList<IPoint> Create()
        {
            var routeBuildType = m_MineConfig.GetRouteBuildType();
            var routeBuildStrategy = m_BuildTypes.GetValueOrDefault(routeBuildType.GetType());

            if (routeBuildStrategy == default)
            {
                //log
                return null;
            }
            
            var mineMap = m_MineConfig.GetMineMap();
           
            var route
                = routeBuildStrategy.BuildRoute(mineMap, routeBuildType);

            return route;
        }
    }
}