using System.Collections.Generic;
using NUnit.Framework;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Tests
{
    public class UnitMovementTests
    {
        private ITransition m_Transition;
        private SpeedConfig m_SpeedConfig;
        private MovementSpeedService m_SpeedService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var wp0 = new WaypointMock(null, Vector3.zero);
            var wp1 = new WaypointMock(null, Vector3.one);
            m_Transition = new TransitionMock(wp0, wp1);

            var speedConfig = new SpeedConfig(0, new List<float> { 1, 2, 3 });
            m_SpeedService = new MovementSpeedService(speedConfig);
        }
        
        // [Test]
        // public void ShouldTransitionPassed()
        // {
        //     var transitionTransfer = new RouteConductor();
        //     var result = transitionTransfer.Conduct(2, m_Transition.From.Position, m_SpeedService, m_Transition);
        //     
        //     var isPassedByRange = m_Transition.To.Position == result.Position;
        //     Assert.IsTrue(isPassedByRange);
        // }
        //
        // [Test]
        // public void ShouldTransitionNotPassed()
        // {
        //     var transitionTransfer = new RouteConductor();
        //     var result = transitionTransfer.Conduct(1, m_Transition.From.Position, m_SpeedService, m_Transition);
        //     
        //     var isNotPassedByRange = m_Transition.To.Position != result.Position;
        //     Assert.IsTrue(isNotPassedByRange);
        // }
    }
}