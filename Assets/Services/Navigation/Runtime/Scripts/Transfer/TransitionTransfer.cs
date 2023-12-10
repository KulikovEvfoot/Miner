using System;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public class TransitionTransfer : ITransitionTransfer
    {
        public TransferInfo Transfer(
            float deltaTime,
            Vector3 currentPosition,
            ISpeedService speedService,
            ITransition transition)
        {
            var nextPosition = transition.To.Position;
            var rangeToNextPosition = Vector3.Distance(nextPosition, currentPosition);

            var speed = speedService.Speed;
            
            var timeToNextPosition = rangeToNextPosition / speed;

            var remainingTime = timeToNextPosition - deltaTime;
            
            if (float.Epsilon > remainingTime)
            {
                //точка пройдена
                return new TransferInfo(nextPosition, Math.Abs(remainingTime), true);
            }

            var direction = transition.GetTransitionDirection();
            var newPosition = currentPosition + (direction * speed * deltaTime);
            
            return new TransferInfo(newPosition, 0, false);
        }
    }
}