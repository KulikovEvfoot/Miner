using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Core.Mine.Runtime;
using Core.Mine.Runtime.Point.Base;
using Core.ResourceExtraction.ResourceExtractor;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using Zenject;

namespace Core.ResourceExtraction
{
    public class ResourcesExtractionController : IInitPhase, IInitPhaseCompleteListener
    {
        private readonly IResourceExtractorFactory m_ResourceExtractorFactory;
        private readonly ResourceExtractionJobFactory m_ResourceExtractionJobFactory;
        private readonly ISpeedService m_SpeedService;
        private readonly SampleGameConfig m_SampleGameConfig;

        private List<IResourceExtractor> m_ResourceExtractors = new List<IResourceExtractor>();

        [Inject]
        public ResourcesExtractionController(
            SampleGameConfig sampleGameConfig,
            AddressablesAssetLoader addressablesAssetLoader, 
            ISpeedService speedService, 
            MineManager mineManager,
            RewardCollectorsController rewardCollectorsController)
        {
            m_SampleGameConfig = sampleGameConfig;
            
            var basePoint = mineManager.GetBasePoint();
            var route = mineManager.CreateRoute();
            m_SpeedService = speedService;
            var coroutineProvider = CoroutineProvider.Instance;
            m_ResourceExtractorFactory = new ResourceExtractorFactory(
                sampleGameConfig.MinerBody,
                addressablesAssetLoader, 
                speedService, 
                coroutineProvider,
                new RouteConductor(), 
                basePoint);
            
            m_ResourceExtractionJobFactory = new ResourceExtractionJobFactory(
                route, 
                rewardCollectorsController);
        }
        
        public async Task Init()
        {
            var countMinersOnStart = m_SampleGameConfig.CountOfMinersAtStart;
            var tasks = new List<Task>();
            for (int i = 0; i < countMinersOnStart; i++)
            {
                tasks.Add(CreateMinerOnInit());
            }

            await Task.WhenAll(tasks);
        }

        public void NotifyOnAllInitComplete()
        {
            var job = m_ResourceExtractionJobFactory.Create();
            foreach (var resourceExtractor in m_ResourceExtractors)
            {
                resourceExtractor.StartJob(job);
            }
        }
        
        private async Task CreateMinerOnInit()
        {
            var resourceExtractor = await m_ResourceExtractorFactory.Create();
            m_ResourceExtractors.Add(resourceExtractor);
        }
        
        public async void CreateMiner()
        {
            var resourceExtractor = await m_ResourceExtractorFactory.Create();
            var job = m_ResourceExtractionJobFactory.Create();
            resourceExtractor.StartJob(job);
            m_ResourceExtractors.Add(resourceExtractor);
        }
        
        public bool CanSpeedUpMiners()
        {
            return m_SpeedService.CanInc();
        }
        
        public void SpeedUpMiners()
        {
            m_SpeedService.Inc();
        }
    }
}