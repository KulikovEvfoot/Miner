using System.Collections.Generic;
using Common;
using Core.ResourceExtraction;
using Core.ResourceExtraction.ResourceExtractor;
using Services.AssetLoader.Runtime;
using Services.Job.Runtime;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using ICoroutineRunner = Common.ICoroutineRunner;

namespace Core
{
    public class ResourceExtractorController : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private WaypointBase m_SpawnPoint;
        
        private IResourceExtractorFactory m_ResourceExtractorFactory;
        private List<IResourceExtractor> m_Miners;
        private EventProducer<IEmployeeFactoryObserver> m_EmployeeFactoryObservers;
        private ISpeedService m_SpeedService;
        private RouteConductor m_RouteConductor;

        public void Init(
            string assetPath,
            IAssetLoader assetLoader, 
            ISpeedService speedService,
            ICoroutineRunner coroutineRunner,
            RouteConductor routeConductor,
            EventProducer<IEmployeeFactoryObserver> employeeFactoryObservers)
        {
            m_RouteConductor = routeConductor;
            m_Miners = new List<IResourceExtractor>();
            m_EmployeeFactoryObservers = employeeFactoryObservers;
            m_SpeedService = speedService;
            m_ResourceExtractorFactory = new ResourceExtractorFactory(
                assetPath, 
                assetLoader,
                speedService, 
                coroutineRunner, 
                m_RouteConductor,
                m_SpawnPoint);
        }

        public void CreateMiner()
        {
            var createdMiner = m_ResourceExtractorFactory.Create();
            if (!createdMiner.IsExist)
            {
                Debug.LogError($"{nameof(ResourceExtractorController)} >>> Can't create miner");
                return;
            }

            var employee = createdMiner.Object;
            m_Miners.Add(employee);
            m_EmployeeFactoryObservers.NotifyAll(obs => obs.NotifyOnEmployeeCreated(employee));
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