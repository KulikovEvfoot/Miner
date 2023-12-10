using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public class RouteMovement : IRouteMovement
    {
        private readonly ICoroutineRunner m_CoroutineRunner;
        private readonly IMovableBody m_MovableBody;
        private readonly ISpeedService m_SpeedService;
        private readonly ITransitionTransfer m_TransitionTransfer;
        
        private Coroutine m_RouteRoutine;

        public RouteMovement(
            IMovableBody body, 
            ISpeedService speedService,
            ICoroutineRunner coroutineRunner,
            ITransitionTransfer transitionTransfer)
        {
            m_MovableBody = body;
            m_SpeedService = speedService;
            m_CoroutineRunner = coroutineRunner;
            m_TransitionTransfer = transitionTransfer;
        }

        public void EnRouteMove(
            IEnumerable<ITransition> transitionsCollection,
            IRouteMoveListener routeMoveListener,
            TransferInfo transferInfo)
        {
            m_RouteRoutine = 
                m_CoroutineRunner.StartCoroutine(
                    PassRouteCoroutine(transitionsCollection, routeMoveListener, transferInfo));
        }

        private IEnumerator PassRouteCoroutine(
            IEnumerable<ITransition> transitionsCollection, 
            IRouteMoveListener routeMoveListener,
            TransferInfo transferInfo)
        {
            var transitions = transitionsCollection as ITransition[] ?? transitionsCollection.ToArray();
            var previousFrameTime = Time.realtimeSinceStartup;
            for (int i = 0; i < transitions.Length;)
            {
                var deltaTime = Math.Abs(previousFrameTime - Time.realtimeSinceStartup) + transferInfo.Time;
                
                transferInfo = m_TransitionTransfer.Transfer(
                    deltaTime,
                    transferInfo.Position,
                    m_SpeedService, 
                    transitions[i]);

                m_MovableBody.Transform.position = transferInfo.Position;

                if (transferInfo.IsTransferComplete)
                {
                    routeMoveListener.NotifyOnTransferComplete(transitions[i]);
                    i++;
                    continue;
                }

                previousFrameTime = Time.realtimeSinceStartup;
                yield return null;
            }
            
            routeMoveListener.NotifyOnRouteComplete(transferInfo.Time);
        }
    }
}