using System.Threading.Tasks;
using Common;
using Core.Mine.Runtime.Point.Base;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractorFactory : IResourceExtractorFactory
    {
        private readonly object m_AssetAddress;
        private readonly AddressablesAssetLoader m_AssetLoader;
        private readonly ISpeedService m_SpeedService;
        private readonly ICoroutineRunner m_CoroutineRunner;
        private readonly IRouteConductor m_RouteConductor;
        private readonly IBasePoint m_SpawnPoint;

        public ResourceExtractorFactory(
            object assetAddress,
            AddressablesAssetLoader assetLoader, 
            ISpeedService speedService,
            ICoroutineRunner coroutineRunner,
            IRouteConductor routeConductor,
            IBasePoint spawnPoint)
        {
            m_AssetAddress = assetAddress;
            m_AssetLoader = assetLoader;
            m_SpeedService = speedService;
            m_CoroutineRunner = coroutineRunner;
            m_RouteConductor = routeConductor;
            m_SpawnPoint = spawnPoint;
        }
        
        public async Task<IResourceExtractor> Create()
        {
            // var instantiateParams = new InstantiationParameters(m_SpawnPoint.Position, Quaternion.identity, null);
            var instantiateParams = new InstantiationParameters(Vector3.zero, Quaternion.identity, null);
            var minerBody = await m_AssetLoader
                .InstantiateAsync<ResourceExtractorBody>(m_AssetAddress, instantiateParams);

            var miner = new ResourceExtractor(minerBody, m_SpeedService, m_CoroutineRunner, m_RouteConductor);
            return miner;
        }
    }
}