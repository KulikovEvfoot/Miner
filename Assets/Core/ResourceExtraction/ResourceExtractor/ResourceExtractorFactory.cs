using Common;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using ICoroutineRunner = Common.ICoroutineRunner;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractorFactory : IResourceExtractorFactory
    {
        private readonly string m_AssetPath;
        private readonly IAssetLoader m_AssetLoader;
        private readonly ISpeedService m_SpeedService;
        private readonly ICoroutineRunner m_CoroutineRunner;
        private readonly ITransitionTransfer m_TransitionTransfer;
        private readonly IWaypoint m_SpawnPoint;

        public ResourceExtractorFactory(
            string assetPath,
            IAssetLoader assetLoader, 
            ISpeedService speedService,
            ICoroutineRunner coroutineRunner,
            ITransitionTransfer transitionTransfer,
            IWaypoint spawnPoint)
        {
            m_AssetPath = assetPath;
            m_AssetLoader = assetLoader;
            m_SpeedService = speedService;
            m_CoroutineRunner = coroutineRunner;
            m_TransitionTransfer = transitionTransfer;
            m_SpawnPoint = spawnPoint;
        }

        public Result<IResourceExtractor> Create()
        {
            var bodyLoadResult = m_AssetLoader.LoadSync<ResourceExtractorBody>(m_AssetPath);
            if (!bodyLoadResult.IsExist)
            {
                return new Result<IResourceExtractor>(null, false);
            }

            var minerBody = Object.Instantiate(bodyLoadResult.Object, m_SpawnPoint.Position, Quaternion.identity);
            var movement = new RouteMovement(minerBody, m_SpeedService, m_CoroutineRunner, m_TransitionTransfer);
            var miner = new ResourceExtractor(minerBody, movement);
            return new Result<IResourceExtractor>(miner, true);
        }
    }
}