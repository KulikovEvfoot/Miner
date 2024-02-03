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
            var lastPassedPointIndex = args.LastPassedPointIndex;
            var currentPosition = args.CurrentPosition;
            var speed = args.SpeedService.Speed;
            var deltaTime = args.DeltaTime;
            
            //блок просчета пройденных петель
            var nextPointIndex = GetNextPointIndex(lastPassedPointIndex, route);
            var distanceToRouteEnd = GetDistanceToRouteEnd(currentPosition, nextPointIndex, route);
            
            var timeToRouteEnd = distanceToRouteEnd / speed;
            var remainingTimeAfterRoute = deltaTime - timeToRouteEnd;
            var isRoutePassed = remainingTimeAfterRoute > 0 - float.Epsilon;

            var countOfCompletedRoutes = 0;
            if (isRoutePassed)
            {
                countOfCompletedRoutes = 1;
                
                currentPosition = route.First().Position;
                lastPassedPointIndex = 0;
                nextPointIndex = GetNextPointIndex(lastPassedPointIndex, route);

                deltaTime = Mathf.Abs(remainingTimeAfterRoute);

                var totalRouteDistance = GetDistanceToRouteEnd(currentPosition, nextPointIndex, route);
                var timeToPassRoute = totalRouteDistance / speed;
                var passedLoops = Mathf.FloorToInt(deltaTime / timeToPassRoute);
                if (passedLoops > 0)
                {
                    countOfCompletedRoutes += passedLoops;
                    deltaTime %= timeToPassRoute;
                }
            }
            
            //блок просчета пройденных точек
            var passedPoints = new List<IPoint>();
            for (int i = nextPointIndex; i < route.Count; i++)
            {
                var nextPoint = route[nextPointIndex];
                var lengthToTransitionEnd = GetDistanceToNextPoint(currentPosition, nextPoint);
                var timeToNextTransition = lengthToTransitionEnd / speed;
                var remainingTimeAfterTransition = deltaTime - timeToNextTransition;
                var isTransitionPassed = remainingTimeAfterTransition > 0 - float.Epsilon;
                if (isTransitionPassed)
                {
                    currentPosition = nextPoint.Position;
                    deltaTime = Math.Abs(remainingTimeAfterTransition);

                    lastPassedPointIndex = nextPointIndex;
                    nextPointIndex = GetNextPointIndex(nextPointIndex, route);
                    
                    passedPoints.Add(nextPoint);
                    continue;
                }
                
                break;
            }

            //блок просчёта за тик
            var lastPassedPoint = route[lastPassedPointIndex];
            var nextPointTick = GetNextPoint(lastPassedPointIndex, route);
            var rangeToNextPosition = Vector3.Distance(nextPointTick.Position, currentPosition);
            var timeToNextPosition = rangeToNextPosition / speed;
            var remainingTimeAfterTick = timeToNextPosition - deltaTime;
            
            if (float.Epsilon > remainingTimeAfterTick)
            {
                //точка пройдена (а так блять не должно быть)
            }

            var direction = PointUtils.GetTransitionDirection(nextPointTick, lastPassedPoint);
            var newPosition = currentPosition + (direction * speed * deltaTime);
            return new RouteConductorResult(lastPassedPointIndex, newPosition, passedPoints, countOfCompletedRoutes);
        }
        
        private float GetDistanceToNextPoint(Vector3 currentPosition, IPoint nextPoint)
        {
            var distanceToNextPoint = Vector3.Distance(nextPoint.Position, currentPosition); 
            return distanceToNextPoint;
        }
        
        private float GetDistanceToRouteEnd(Vector3 currentPosition, int nextPointIndex, IReadOnlyList<IPoint> route)
        {
            var distanceToNextPoint = GetDistanceToNextPoint(currentPosition, route[nextPointIndex]);
            if (nextPointIndex == 0)
            {
                return distanceToNextPoint;
            }

            var lastPassedPoint = nextPointIndex;
            nextPointIndex = GetNextPointIndex(nextPointIndex, route);
            
            for (int i = nextPointIndex; i < route.Count; i++)
            {
                distanceToNextPoint += PointUtils.GetTransitionLength(route[lastPassedPoint], route[nextPointIndex]);
            }

            return distanceToNextPoint;
        }
        
        private int GetNextPointIndex(int lastPassedPointIndex, IReadOnlyList<IPoint> route)
        {
            var nextIndex = lastPassedPointIndex + 1;
            if (nextIndex == route.Count)
            {
                return 0;
            }

            return nextIndex;
        }

        private IPoint GetNextPoint(int lastPassedPointIndex, IReadOnlyList<IPoint> route)
        {
            return route[GetNextPointIndex(lastPassedPointIndex, route)];
        }
    }
}