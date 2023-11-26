using Common;
using Common.AssetLoader.Runtime;
using Common.Currency.Runtime;
using Common.EventProducer.Runtime;
using Common.Job.Runtime;
using Common.Moving.Runtime;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime;
using Core.Mine.Runtime.RouteBuilder;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private const string m_MinerAssetPath = "miner";

        [SerializeField] private MapRouter m_MapRouter;
        [SerializeField] private RewardCollectorsController m_RewardCollectorsController;
        [SerializeField] private MinersController m_MinersController;
        [SerializeField] private ResourceExtractionController m_ResourceExtractionController;
        [SerializeField] private CurrencyController m_CurrencyController;
        [SerializeField] private SampleViewController m_SampleViewController;

        [SerializeField] private long m_StartGoldCurrencyCount;
        [SerializeField] private MovementSpeedConfig m_MovementSpeedConfig;
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
            
            IRouteBuilder routeBuilder = new DijkstrasRouteBuilder(m_MapRouter);
            
            m_CurrencyController.Init();
            ICurrencyController goldCurrencyController = m_CurrencyController.GoldCurrencyController;
            goldCurrencyController.AddValue(m_StartGoldCurrencyCount);
            
            m_RewardCollectorsController.Init(m_CurrencyController.RewardCollectors);
            
            m_ResourceExtractionController.Init(
                m_MapRouter, 
                m_RewardCollectorsController,
                m_JobFactoryObservers, 
                m_JobInfoObserver);
            
            IMovementSpeedService minerMovementSpeedService = new MovementSpeedService(m_MovementSpeedConfig);
            IUnitMovementFactory unitMovementFactory = new UnitMovementFactory(minerMovementSpeedService, this);

            m_MinersController.Init(m_MinerAssetPath, assetLoader, routeBuilder, unitMovementFactory, m_EmployeeFactoryObservers);

            m_SampleViewController.Init(m_MinersController, goldCurrencyController, m_SamplePriceConfig);
        }
    }
}