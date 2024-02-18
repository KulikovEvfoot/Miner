using System.Collections.Generic;
using NUnit.Framework;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class RouteConductorTests_HardRoute
    {
        private SpeedConfig m_SpeedConfig;
        private ISpeedService m_SpeedService;
        private IRouteConductor m_RouteConductor;
        private IRoute m_Route;

        private int m_TimeToPassRoute;
        private List<int> m_Highways;
        
        //# # # # # # #
        //# # # B ↔ C #
        //# # # ↕ # # #
        //# G # A ↔ D #
        //# ↕ # ↕ # # #
        //# F ↔ E # # #
        //# # # # # # #
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var a = new PointMock(0, new[] { 1, 3 }, Vector3.zero); //Base Point
            var b = new PointMock(1, new[] { 0, 2 }, new Vector3(0, 1, 0));
            var c = new PointMock(2, new[] { 1 }, new Vector3(1, 1, 0));
            var d = new PointMock(3, new[] { 0 }, new Vector3(1, 0, 0));
            var e = new PointMock(4, new[] { 0, 5 }, new Vector3(0, -1, 0));
            var f = new PointMock(5, new[] { 4, 6 }, new Vector3(-1, -1, 0));
            var g = new PointMock(6, new[] { 5 }, new Vector3(-1, 0, 0));
            
            var highway0 = new HighwayMock(new List<IPoint> { a, b, c, b, a });
            var highway1 = new HighwayMock(new List<IPoint> { a, d, a });
            var highway2 = new HighwayMock(new List<IPoint> { a, e, f, g, f, e, a });
            
            m_Route = new RouteMock(new List<IHighway> { highway0, highway1, highway2 });
            m_TimeToPassRoute = 12;
            m_Highways = new List<int> { 4, 2, 6 };
            m_RouteConductor = new RouteConductor();
        }

        [SetUp]
        public void Setup()
        {
            var speedConfig = new SpeedConfig(0, new List<float> { 1, 100000});
            m_SpeedService = new MovementSpeedService(speedConfig);
        }

        [Test]
        public void ShouldPass_1_Highway()
        {
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed,
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    4f)));

            var highwayIndex = 1;
            var currentPointIndex = 0;
            var startPoint = m_Route.Highways[highwayIndex].Points[currentPointIndex].Position;
            
            Assert.AreEqual(result.PassedRoutesCount, 0);
            Assert.AreEqual(result.PassedHighways.Count, 1);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, 0);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, 1);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, startPoint);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, currentPointIndex);
        }
        
        [Test]
        public void ShouldPass_3_Route_2_Highway()
        {
            var thirdHighwayPassing = 2;
            var time = (m_TimeToPassRoute * 3) + m_Highways[0] + m_Highways[1] + thirdHighwayPassing;
            
            var result = m_RouteConductor.Conduct(new RouteConductorArgs(
                m_Route,
                m_SpeedService.Speed,
                new RouteTravelInfo(
                    0,
                    0,
                    m_Route.Highways[0].Points[0].Position, 
                    time)));
            
            var currentPointIndex = 2;
            var currentHighwayIndex = 2;
            var currentPosition = m_Route.Highways[currentHighwayIndex].Points[currentPointIndex].Position;

            Assert.AreEqual(result.PassedRoutesCount, 3);
            Assert.AreEqual(result.PassedHighways.Count, 2);
            Assert.AreEqual(result.PassedPointsOnCurrentHighway.Count, thirdHighwayPassing);
            Assert.AreEqual(result.RouteTravelInfo.CurrentHighwayIndex, currentHighwayIndex);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPosition, currentPosition);
            Assert.AreEqual(result.RouteTravelInfo.CurrentPointIndex, thirdHighwayPassing);
        }
    }
}