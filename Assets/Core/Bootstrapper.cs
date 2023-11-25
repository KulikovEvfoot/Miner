using Common;
using Common.AssetLoader.Runtime;
using Common.Job.Runtime;
using Common.Navigation.Runtime;
using Core.Currency.Runtime;
using Core.EventProducer.Runtime;
using Core.Job.Runtime;
using Core.NavigationSystem.Runtime;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private MapRouter m_MapRouter;
        [SerializeField] private RewardCollectorsController m_RewardCollectorsController;
        [SerializeField] private MinersController m_MinersController;
        [SerializeField] private ResourceExtractionController m_ResourceExtractionController;

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
            
            m_RewardCollectorsController.Init();
            
            m_ResourceExtractionController.Init(
                m_MapRouter, 
                m_RewardCollectorsController,
                m_JobFactoryObservers, 
                m_JobInfoObserver);
            
            m_MinersController.Init(assetLoader, routeBuilder, m_EmployeeFactoryObservers);
            
        }
    }
}