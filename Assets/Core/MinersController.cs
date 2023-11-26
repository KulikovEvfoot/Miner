using System.Collections.Generic;
using Common;
using Common.AssetLoader.Runtime;
using Common.EventProducer.Runtime;
using Common.Job.Runtime;
using Common.Moving.Runtime;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime;
using Common.Navigation.Runtime.Waypoint;
using Core.Units.Runtime.Miner;
using UnityEngine;

namespace Core
{
    public class MinersController : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private WaypointBase m_SpawnPoint;
        
        private IMinerFactory m_MinerFactory;
        private IAssetLoader m_AssetLoader;
        private IRouteNavigator m_RouteNavigator;
        private IUnitMovementFactory m_UnitMovementFactory;

        private List<IMiner> m_Miners;
        private EventProducer<IEmployeeFactoryObserver> m_EmployeeFactoryObservers;
        private IMovementSpeedService m_MovementSpeedService;

        public void Init(
            string assetPath,
            IAssetLoader assetLoader, 
            IRouteNavigator routeNavigator,
            IMovementSpeedService movementSpeedService,
            IUnitMovementFactory unitMovementFactory,
            EventProducer<IEmployeeFactoryObserver> employeeFactoryObservers)
        {
            m_Miners = new List<IMiner>();
            m_EmployeeFactoryObservers = employeeFactoryObservers;
            m_MovementSpeedService = movementSpeedService;
            m_MinerFactory = new MinerFactory(assetPath, assetLoader, routeNavigator, unitMovementFactory, m_SpawnPoint);
        }

        public void CreateMiner()
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

        public bool CanSpeedUpMiners()
        {
            return m_MovementSpeedService.CanInc();
        }
        
        public void SpeedUpMiners()
        {
            m_MovementSpeedService.Inc();
        }
    }
}