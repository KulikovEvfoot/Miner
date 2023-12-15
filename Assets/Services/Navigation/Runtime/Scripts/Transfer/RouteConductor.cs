using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public class RouteConductor : IRouteConductor
    {
        public RouteConductorResult Conduct(RouteConductorArgs args)
        {
            var route = args.Route;
            var currentTransitionIndex = args.TransitionIndex;
            var currentPosition = args.Position;
            var speed = args.SpeedService.Speed;
            var deltaTime = args.DeltaTime;
            var routeMoveListener =  args.RouteMoveListener;
            
            //блок просчета пройденных петель
            var lengthToRouteEnd = GetLengthToRouteEnd(currentPosition, currentTransitionIndex, route);
            
            var timeToRouteEnd = lengthToRouteEnd / speed;
            var remainingTimeAfterRoute = deltaTime - timeToRouteEnd;
            var isRoutePassed = remainingTimeAfterRoute > 0 - float.Epsilon;

            if (isRoutePassed)
            {
                var countOfCompletedRoutes = 1;
                
                currentPosition = route.First().From.Position;
                currentTransitionIndex = 0;
                deltaTime = Mathf.Abs(remainingTimeAfterRoute);

                var totalRouteLength = GetLengthToRouteEnd(currentPosition, currentTransitionIndex, route);
                var timeToPassRoute = totalRouteLength / speed;
                var passedLoops = Mathf.FloorToInt(deltaTime / timeToPassRoute);
                if (passedLoops > 0)
                {
                    countOfCompletedRoutes += passedLoops;
                    deltaTime %= timeToPassRoute;
                }
                
                routeMoveListener.NotifyOnRouteComplete(route, countOfCompletedRoutes);
            }
            
            //блок просчета пройденных переходов
            var passedTransitions = new List<ITransition>();
            for (int i = currentTransitionIndex; i < route.Count; i++)
            {
                var currentTransition = route[currentTransitionIndex];
                var lenghtToTransitionEnd = GetLenghtToTransitionEnd(currentPosition, currentTransition);
                var timeToNextTransition = lenghtToTransitionEnd / speed;
                var remainingTimeAfterTransition = deltaTime - timeToNextTransition;
                var isTransitionPassed = remainingTimeAfterTransition > 0 - float.Epsilon;
                if (isTransitionPassed)
                {
                    currentPosition = currentTransition.To.Position;
                    deltaTime = Math.Abs(remainingTimeAfterTransition);

                    currentTransitionIndex++;
                    
                    passedTransitions.Add(currentTransition);
                    continue;
                }

                if (passedTransitions.Count > 0)
                {
                    routeMoveListener.NotifyOnTransitionComplete(passedTransitions);
                }
                
                break;
            }

            //блок просчёта за тик
            var tickTransition = route[currentTransitionIndex];
            var nextPosition = tickTransition.To.Position;
            var rangeToNextPosition = Vector3.Distance(nextPosition, currentPosition);
            var timeToNextPosition = rangeToNextPosition / speed;
            var remainingTimeAfterTick = timeToNextPosition - deltaTime;
            
            if (float.Epsilon > remainingTimeAfterTick)
            {
                //точка пройдена (а так блять не должно быть)
            }
            
            var direction = tickTransition.GetTransitionDirection();
            var newPosition = currentPosition + (direction * speed * deltaTime);
            var tickInfo = new RouteTickInfo(newPosition);
            routeMoveListener.Tick(tickInfo);

            return new RouteConductorResult(currentTransitionIndex, newPosition);
        }
        
        private float GetLenghtToTransitionEnd(Vector3 currentPosition, ITransition transition)
        {
            var lengthToNextTransition = Vector3.Distance(transition.To.Position, currentPosition); 
            return lengthToNextTransition;
        }
        
        private float GetLengthToRouteEnd(Vector3 currentPosition, int currentIndex, IList<ITransition> transitions)
        {
            var lenghtToTransitionEnd = GetLenghtToTransitionEnd(currentPosition, transitions[currentIndex]);
            var lengthToRouteEnd = lenghtToTransitionEnd;
            var nextTransitionIndex = GetNextTransitionIndex(currentIndex, transitions);
            if (nextTransitionIndex == 0)
            {
                return lengthToRouteEnd;
            }
            
            for (int i = nextTransitionIndex; i < transitions.Count; i++)
            {
                lengthToRouteEnd += transitions[i].GetTransitionLenght();
            }

            return lengthToRouteEnd;
        }
        
        private int GetNextTransitionIndex(int currentIndex, IList<ITransition> transitions)
        {
            var nextIndex = currentIndex + 1;
            if (transitions.Count == currentIndex + 1)
            {
                return 0;
            }

            return nextIndex;
        }
    }
}