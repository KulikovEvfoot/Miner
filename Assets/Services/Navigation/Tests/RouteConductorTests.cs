using System.Collections.Generic;
using NUnit.Framework;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class RouteConductorTests
    {
        private SpeedConfig m_SpeedConfig;
        private MovementSpeedService m_SpeedService;
        private RouteConductor m_RouteConductor;
        private List<ITransition> m_Route;
        
        private RouteMoveListenerMock m_RouteMoveListenerMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var baseWp = new WaypointMock(Vector3.zero);
            var wp1 = new WaypointMock(new Vector3(0, 1, 0));
            m_Route = new List<ITransition>
            {
                new TransitionMock(baseWp, wp1),
                new TransitionMock(wp1, baseWp)
            };
            
            m_RouteConductor = new RouteConductor();
        }

        [SetUp]
        public void Setup()
        {
            var speedConfig = new SpeedConfig(0, new List<float> { 1, 100000});
            m_SpeedService = new MovementSpeedService(speedConfig);
            
            m_RouteMoveListenerMock = new RouteMoveListenerMock();
        }

        [Test]
        public void ShouldPass_1_Transition()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                TransitionIndex = 0,
                Position = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 1f,
                RouteMoveListener = m_RouteMoveListenerMock
            });

            var listenerResult = m_RouteMoveListenerMock;
            Assert.AreEqual(listenerResult.Position, new Vector3(0, 1, 0));
            Assert.AreEqual(listenerResult.PassedTransitionCount, 1);
            Assert.AreEqual(listenerResult.PassedRoutesCount, 0);
        }
        
        [Test]
        public void ShouldPass_1_Route()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                TransitionIndex = 0,
                Position = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2f,
                RouteMoveListener = m_RouteMoveListenerMock
            });

            var listenerResult = m_RouteMoveListenerMock;
            Assert.AreEqual(listenerResult.Position, Vector3.zero);
            Assert.AreEqual(listenerResult.PassedTransitionCount, 0);
            Assert.AreEqual(listenerResult.PassedRoutesCount, 1);
        }

        [Test]
        public void ShouldPass_1_000_000_Routes()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                TransitionIndex = 0,
                Position = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2000000f,
                RouteMoveListener = m_RouteMoveListenerMock
            });

            var listenerResult = m_RouteMoveListenerMock;
            Assert.AreEqual(listenerResult.Position, Vector3.zero);
            Assert.AreEqual(listenerResult.PassedTransitionCount, 0);
            Assert.AreEqual(listenerResult.PassedRoutesCount, 1000000);
        }
        
        [Test]
        public void ShouldPass_1_000_000_Routes_And_1_Transition()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                TransitionIndex = 0,
                Position = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2000001f,
                RouteMoveListener = m_RouteMoveListenerMock
            });

            var listenerResult = m_RouteMoveListenerMock;
            Assert.AreEqual(listenerResult.Position, new Vector3(0, 1, 0));
            Assert.AreEqual(listenerResult.PassedTransitionCount, 1);
            Assert.AreEqual(listenerResult.PassedRoutesCount, 1000000);
        }
        
        // [Test]
        // public void ShouldPass_100_000_Times_BySpeed_100_000()
        // {
        //     m_SpeedService.Inc();
        //     
        //     var result = m_RouteConductor.Conduct(new RouteConductorArgs
        //     {
        //         Route = m_Route,
        //         TransitionIndex = 0,
        //         Position = Vector3.zero,
        //         SpeedService = m_SpeedService,
        //         DeltaTime = 100000f,
        //         RouteMoveListener = m_RouteMoveListenerMock
        //     });
        //
        //     var listenerResult = m_RouteMoveListenerMock;
        //     Assert.AreEqual(listenerResult.Position, Vector3.zero);
        //     Assert.AreEqual(listenerResult.PassedTransitionCount, 0);
        //     Assert.AreEqual(listenerResult.PassedRoutesCount, ??);
        // }
    }
}