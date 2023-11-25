using System.Collections.Generic;
using Common;
using Common.AssetLoader;
using Common.AssetLoader.Runtime;
using Common.EventProducer;
using Common.EventProducer.Runtime;
using Common.Job;
using Common.Job.Runtime;
using Common.Moving.Runtime;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.Mine.Runtime;
using Core.Units.Runtime.Miner;
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
            IMovementSpeedService movementSpeedService,
            EventProducer<IEmployeeFactoryObserver> employeeFactoryObservers)
        {
            m_Miners = new List<Miner>();
            m_EmployeeFactoryObservers = employeeFactoryObservers;
            m_MovementSpeedService = movementSpeedService;
            
            var unitMovementFactory = new UnitMovementFactory(m_MovementSpeedService, this);
            
            m_MinerFactory = new MinerFactory(AssetPath, assetLoader, routeBuilder, unitMovementFactory, m_SpawnPoint);
            
            m_CreateMiner.onClick.AddListener(CreateMiner);
        }

        private void CreateMiner()
        {
            var createdMiner = m_MinerFactory.Create();
            if (!createdMiner.IsExist)
            {
                Debug.LogError($"{nameof(MinersController)} >>> Can't create miner");
                return;
            }

            var employee = createdMiner.Object;
            m_Miners.Add(employee);
            m_EmployeeFactoryObservers.NotifyAll(obs => obs.NotifyOnEmployeeCreated(employee));
        }
    }
}