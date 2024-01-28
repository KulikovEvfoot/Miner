using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Core.Mine.Runtime;
using Core.Mine.Runtime.Point.Base;
using Core.ResourceExtraction.ResourceExtractor;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using Zenject;

namespace Core.ResourceExtraction
{
    public class ResourcesExtractionController : IInitPhase
    {
        private readonly IResourceExtractorFactory m_ResourceExtractorFactory;
        private readonly ResourceExtractionJobFactory m_ResourceExtractionJobFactory;
        private readonly ISpeedService m_SpeedService;

        private List<IResourceExtractor> m_ResourceExtractors = new List<IResourceExtractor>();

        [Inject]
        public ResourcesExtractionController(
            SampleGameConfig sampleGameConfig,
            AddressablesAssetLoader addressablesAssetLoader, 
            ISpeedService speedService, 
            MineMap mineMap,
            RewardCollectorsController rewardCollectorsController)
        {
            IBasePoint basePoint = mineMap.GetBasePoint();
            var transitions = mineMap.GetTransitions();
            
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
                transitions, 
                rewardCollectorsController);
        }
        
        public async Task Init()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(CreateMiner());
            }

            await Task.WhenAll(tasks);
        }

        
        public async Task CreateMiner()
        {
            var resourceExtractor = await m_ResourceExtractorFactory.Create();
            var job = m_ResourceExtractionJobFactory.Create();
            resourceExtractor.StartJob(job);
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