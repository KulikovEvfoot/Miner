using System;
using System.Collections;
using System.Collections.Generic;
using Common.Moving.Runtime.Speed;
using Common.Navigation.Runtime.Waypoint;
using UnityEngine;

namespace Common.Moving.Runtime
{
    public class UnitMovement : IUnitMovement
    {
        private readonly IMovementSpeedService m_MovementSpeedService;
        private readonly ICoroutineRunner m_CoroutineRunner;
        
        private IMovableBody m_MovableBody;
        private Coroutine m_EnRouteMovingRoutine;
        private Coroutine m_MoveToWaypoint;
        private Action<string> m_OperationResult;
        
        public IMovementSpeedService MovementSpeedService => m_MovementSpeedService;
        
        public UnitMovement(
            IMovementSpeedService movementSpeedService, 
            ICoroutineRunner coroutineRunner)
        {
            m_MovementSpeedService = movementSpeedService;
            m_CoroutineRunner = coroutineRunner;
        }
        
        public void EnRouteMove(
            IMovableBody movableBody,
            List<IWaypoint> route, 
            Action<string> operationResult = null)
        {
            m_MovableBody = movableBody;
            m_OperationResult = operationResult;
            m_EnRouteMovingRoutine = m_CoroutineRunner.StartCoroutine(EnRouteRoutine(route));
        }
        
        private IEnumerator EnRouteRoutine(List<IWaypoint> route)
        {
            foreach (var waypoint in route)
            {
                m_MoveToWaypoint = m_CoroutineRunner.StartCoroutine(MoveTo(waypoint));
                yield return m_MoveToWaypoint;
            }
            
            m_OperationResult?.Invoke(UnitMovementEnvironment.CompleteKey);
        }

        private IEnumerator MoveTo(IWaypoint waypoint)
        {
            while (m_MovableBody.Transform.position != waypoint.Position)
            {
                m_MovableBody.Transform.position 
                    = Vector3.MoveTowards(
                        m_MovableBody.Transform.position, 
                        waypoint.Position,
                        m_MovementSpeedService.Speed);
                
                yield return new WaitForFixedUpdate();
            }
            
            m_MovableBody.SetWaypoint(waypoint);
        }
    }
}