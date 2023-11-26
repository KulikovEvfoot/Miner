using Common;
using Common.AssetLoader;
using Common.AssetLoader.Runtime;
using Common.Moving.Runtime;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Core.Units.Runtime.Miner
{
    public class MinerFactory : IMinerFactory
    {
        private readonly string m_AssetPath;
        private readonly IAssetLoader m_AssetLoader;
        private readonly IRouteNavigator m_RouteNavigator;
        private readonly IUnitMovementFactory m_UnitMovementFactory;
        private readonly IWaypoint m_SpawnPoint;

        public MinerFactory(
            string assetPath,
            IAssetLoader assetLoader, 
            IRouteNavigator routeNavigator, 
            IUnitMovementFactory unitMovementFactory, 
            IWaypoint spawnPoint)
        {
            m_AssetPath = assetPath;
            m_AssetLoader = assetLoader;
            m_RouteNavigator = routeNavigator;
            m_UnitMovementFactory = unitMovementFactory;
            m_SpawnPoint = spawnPoint;
        }

        public Result<IMiner> Create()
        {
            var bodyLoadResult = m_AssetLoader.LoadSync<MinerBody>(m_AssetPath);
            if (!bodyLoadResult.IsExist)
            {
                return new Result<IMiner>(null, false);
            }

            var unitMovement = m_UnitMovementFactory.Create();
            var minerBody = Object.Instantiate(bodyLoadResult.Object, m_SpawnPoint.Transform);
            minerBody.SetWaypoint(m_SpawnPoint);
            var miner = new Miner(minerBody, m_RouteNavigator, unitMovement);
            return new Result<IMiner>(miner, true);
        }
    }
}