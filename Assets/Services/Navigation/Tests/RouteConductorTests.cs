using System.Collections.Generic;
using System.Linq;
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
        private List<IPoint> m_Route;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var baseWp = new Point(0, new[] { 1 }, Vector3.zero);
            var wp1 = new Point(1, new[] { 0 }, new Vector3(0, 1, 0));
            m_Route = new List<IPoint> { baseWp, wp1, baseWp };
            
            m_RouteConductor = new RouteConductor();
        }

        [SetUp]
        public void Setup()
        {
            var speedConfig = new SpeedConfig(0, new List<float> { 1, 100000});
            m_SpeedService = new MovementSpeedService(speedConfig);
        }

        [Test]
        public void ShouldPass_1_Point()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                LastPassedPointIndex = 0,
                CurrentPosition = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 1f,
            });

            Assert.AreEqual(result.CurrentPosition, new Vector3(0, 1, 0));
            Assert.AreEqual(result.PassedPoints.Count(), 1);
            Assert.AreEqual(result.PassedRoutesCount, 0);
        }
        
        [Test]
        public void ShouldPass_1_Route()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                LastPassedPointIndex = 0,
                CurrentPosition = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2f,
            });

            Assert.AreEqual(result.CurrentPosition, Vector3.zero);
            Assert.AreEqual(result.PassedPoints.Count(), 0);
            Assert.AreEqual(result.PassedRoutesCount, 1);
        }

        [Test]
        public void ShouldPass_1_000_000_Routes()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                LastPassedPointIndex = 0,
                CurrentPosition = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2000000f,
            });

            Assert.AreEqual(result.CurrentPosition, Vector3.zero);
            Assert.AreEqual(result.PassedPoints.Count(), 0);
            Assert.AreEqual(result.PassedRoutesCount, 1000000);
        }
        
        [Test]
        public void ShouldPass_1_000_000_Routes_And_1_Transition()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs
            {
                Route = m_Route,
                LastPassedPointIndex = 0,
                CurrentPosition = Vector3.zero,
                SpeedService = m_SpeedService,
                DeltaTime = 2000001f,
            });

            Assert.AreEqual(result.CurrentPosition, new Vector3(0, 1, 0));
            Assert.AreEqual(result.PassedPoints.Count(), 1);
            Assert.AreEqual(result.PassedRoutesCount, 1000000);
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