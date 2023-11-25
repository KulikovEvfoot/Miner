using System.Collections.Generic;
using Common;
using Common.AssetLoader.Runtime;
using Common.Job.Runtime;
using Common.Job.Runtime.ResourceExtraction;
using Common.Moving.Runtime;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.EventProducer.Runtime;
using Core.Mine.Runtime;
using Core.NavigationSystem.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class MinersController : MonoBehaviour, ICoroutineRunner
    {
        private const string AssetPath = "miner";
        
        [SerializeField] private Button m_CreateMiner;
        [SerializeField] private WaypointBase m_SpawnPoint;
        
        private IMinerFactory m_MinerFactory;
        private IAssetLoader m_AssetLoader;
        private IRouteBuilder m_RouteBuilder;
        private IMovementSpeedService m_MovementSpeedService;

        private List<Miner> m_Miners;
        private EventProducer<IEmployeeFactoryObserver> m_EmployeeFactoryObservers;

        public List<Miner> Miners => m_Miners;
        
        public void Init(
            IAssetLoader assetLoader, 
            IRouteBuilder routeBuilder,
            EventProducer<IEmployeeFactoryObserver> employeeFactoryObservers)
        {
            m_Miners = new List<Miner>();
            m_EmployeeFactoryObservers = employeeFactoryObservers;
            var config = new MovementSpeedConfig
            {
                StartSpeedIndex = 0,
                SpeedSettings = new List<float>{0.1f,0.2f,0.3f,0.4f,0.5f}
            };

            m_MovementSpeedService = new MovementSpeedService(config);
            var unitMovementFactory = new UnitMovementFactory(m_MovementSpeedService, this);
            
            m_MinerFactory = new MinerFactory(AssetPath, assetLoader, routeBuilder, unitMovementFactory, m_SpawnPoint);
            
            m_CreateMiner.onClick.AddListener(CreateMiner);
        }

        private void CreateMiner()
        {
            var createdMiner = m_MinerFactory.Create();
            if (!createdMiner.IsExist)
            {
                //pizdec
                return;
            }

            var employee = createdMiner.Object;
            m_Miners.Add(employee);
            m_EmployeeFactoryObservers.NotifyAll(obs => obs.NotifyOnEmployeeCreated(employee));
        }
    }
}