using Common;
using Services.AssetLoader.Runtime;
using Services.Currency.Runtime;
using Services.Job.Runtime;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private const string m_MinerAssetPath = "miner";

        [SerializeField] private RewardCollectorsController m_RewardCollectorsController;
        [SerializeField] private ResourceExtractorController m_ResourceExtractorController;
        [SerializeField] private ResourceExtractionJobController m_ResourceExtractionJobController;
        [SerializeField] private CurrencyController m_CurrencyController;
        [SerializeField] private SampleViewController m_SampleViewController;

        [SerializeField] private long m_StartGoldCurrencyCount;
        [SerializeField] private SpeedConfig speedConfig;
        [SerializeField] private SamplePriceConfig m_SamplePriceConfig;
        
        private readonly EventProducer<IJobFactoryObserver> m_JobFactoryObservers
            = new EventProducer<IJobFactoryObserver>();
        
        private readonly EventProducer<IEmployeeFactoryObserver> m_EmployeeFactoryObservers 
            = new EventProducer<IEmployeeFactoryObserver>();
        
        private readonly EventProducer<IJobInfoObserver> m_JobInfoObserver 
            = new EventProducer<IJobInfoObserver>();

        private readonly JobService m_JobService = new JobService();
        
        private void Start()
        {
            m_JobFactoryObservers.Attach(m_JobService);
            m_EmployeeFactoryObservers.Attach(m_JobService);
            m_JobInfoObserver.Attach(m_JobService);
            
            IAssetLoader assetLoader = new ResourceLoader();
            
            m_CurrencyController.Init();
            ICurrencyController goldCurrencyController = m_CurrencyController.GoldCurrencyController;
            goldCurrencyController.AddValue(m_StartGoldCurrencyCount);
            
            m_RewardCollectorsController.Init(m_CurrencyController.RewardCollectors);
            
            m_ResourceExtractionJobController.Init(
                m_RewardCollectorsController,
                m_JobFactoryObservers, 
                m_JobInfoObserver);
            
            ISpeedService minerSpeedService = new MovementSpeedService(speedConfig);

            var routeConductor = new RouteConductor();
            
            m_ResourceExtractorController.Init(
                m_MinerAssetPath, 
                assetLoader, 
                minerSpeedService,
                this, 
                routeConductor,
                m_EmployeeFactoryObservers);

            m_SampleViewController.Init(
                m_ResourceExtractorController, 
                m_ResourceExtractionJobController,
                goldCurrencyController, 
                m_SamplePriceConfig);
            
            m_ResourceExtractorController.CreateMiner();
            m_ResourceExtractionJobController.CreateJob();
        }
    }
}