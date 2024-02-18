using System.Collections.Generic;
using NUnit.Framework;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class RouteConductorTests_SimpleHighway
    {
        private SpeedConfig m_SpeedConfig;
        private ISpeedService m_SpeedService;
        private IRouteConductor m_RouteConductor;
        private IRoute m_Route;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var baseWp = new PointMock(0, new[] { 1 }, Vector3.zero);
            var wp1 = new PointMock(1, new[] { 0 }, new Vector3(0, 1, 0));
            var highway1 = new HighwayMock(new List<IPoint> { baseWp, wp1, baseWp });
            m_Route = new RouteMock(new List<IHighway> { highway1 });
            
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
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed,
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    1f)));

            var highwayIndex = 0;
            var currentPointIndex = 1;
            var secondPoint = m_Route.Highways[highwayIndex].Points[currentPointIndex].Position;
            
            Assert.AreEqual(result.PassedRoutesCount, 0);
            Assert.AreEqual(result.PassedHighways.Count, 0);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, 1);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, highwayIndex);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, secondPoint);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, currentPointIndex);
        }
        
        [Test]
        public void ShouldPass_1_Route()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed, 
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    2f)));

            var passedRoutes = 1;
            var highwayIndex = 0;
            var currentPointIndex = 0;
            var startPoint = m_Route.Highways[highwayIndex].Points[currentPointIndex].Position;
            
            Assert.AreEqual(result.PassedRoutesCount, passedRoutes);
            Assert.AreEqual(result.PassedHighways.Count, 0);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, 0);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, highwayIndex);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, startPoint);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, currentPointIndex);
        }
        
        [Test]
        public void ShouldPass_1_000_000_Routes()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed, 
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    2000000f)));
            
            var passedRoutes = 1000000;
            var highwayIndex = 0;
            var currentPointIndex = 0;
            var startPoint = m_Route.Highways[highwayIndex].Points[currentPointIndex].Position;
            
            Assert.AreEqual(result.PassedRoutesCount, passedRoutes);
            Assert.AreEqual(result.PassedHighways.Count, 0);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, 0);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, highwayIndex);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, startPoint);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, currentPointIndex);
        }
        
        [Test]
        public void ShouldPass_1_000_000_Routes_And_1_Point()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed, 
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    2000001f)));
        
            var passedRoutes = 1000000;
            var highwayIndex = 0;
            var currentPointIndex = 1;
            var second = m_Route.Highways[highwayIndex].Points[currentPointIndex].Position;
            
            Assert.AreEqual(result.PassedRoutesCount, passedRoutes);
            Assert.AreEqual(result.PassedHighways.Count, 0);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, 1);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, highwayIndex);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, second);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, currentPointIndex);
        }
    }
}