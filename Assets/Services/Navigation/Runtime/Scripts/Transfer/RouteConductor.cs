using System;
using System.Collections;
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
            var speed = args.Speed;
            var routeTravelInfo = args.RouteTravelInfo;
            
            var routeFullLength = GetRouteLength(args.Route);
            var routePassingTime = routeFullLength / args.Speed;
            var countOfCompletedRoutes = Mathf.FloorToInt(routeTravelInfo.DeltaTime / routePassingTime);
            if (countOfCompletedRoutes > 0)
            {
                routeTravelInfo.UpdateDeltaTime(routeTravelInfo.DeltaTime % routePassingTime);
            }
            
            var highwayResult = GetHighwaysByTime(args.Route, args.Speed, routeTravelInfo);

            if (highwayResult.IsPassed)
            {
                routeTravelInfo = highwayResult.RouteTravelInfo;
            }

            var passedPointsOnCurrentHighway = new List<IPoint>();
            var currentPoints = route.Highways[routeTravelInfo.CurrentHighwayIndex].Points;
            for (int i = routeTravelInfo.CurrentPointIndex; i < currentPoints.Count; i++)
            {
                var nextPointInHighwayIndex = GetNextIndex(currentPoints, routeTravelInfo.CurrentPointIndex);
                var nextPointInHighway = currentPoints[nextPointInHighwayIndex];
                var lengthToNextPoint 
                    = GetDistanceToNextPoint(routeTravelInfo.CurrentPosition, nextPointInHighway);
                
                var timeToNextPoint = lengthToNextPoint / speed;
                var remainingTimeAfterTransition = routeTravelInfo.DeltaTime - timeToNextPoint;
                var isTransitionPassed = remainingTimeAfterTransition > 0 - float.Epsilon;
                
                if (isTransitionPassed)
                {
                    passedPointsOnCurrentHighway.Add(nextPointInHighway);
                    
                    routeTravelInfo = new RouteTravelInfo(
                        routeTravelInfo.CurrentHighwayIndex,
                        nextPointInHighwayIndex, 
                        currentPoints[nextPointInHighwayIndex].Position,
                        remainingTimeAfterTransition);
                    
                    continue;
                }
                
                break;
            }

            var nextPointIndex = GetNextIndex(currentPoints, routeTravelInfo.CurrentPointIndex);
            var nextPoint = currentPoints[nextPointIndex];
            var currentPoint = currentPoints[routeTravelInfo.CurrentPointIndex];
            var direction = NavigationUtils.GetTransitionDirection(nextPoint, currentPoint);
            var newPosition = routeTravelInfo.CurrentPosition + (direction * speed * routeTravelInfo.DeltaTime);

            var resultRouteTravelInfo = new RouteTravelInfo(
                routeTravelInfo.CurrentHighwayIndex,
                routeTravelInfo.CurrentPointIndex,
                newPosition,
                0);
            
            return new RouteConductorResult(
                route,
                countOfCompletedRoutes,
                highwayResult.Highways,
                passedPointsOnCurrentHighway,
                resultRouteTravelInfo);
        }

        private HighwayInfo GetHighwaysByTime(IRoute route, float speed, RouteTravelInfo routeTravelInfo)
        {
            var highwayResult = new HighwayInfo(routeTravelInfo, new List<IHighway>());
            while (true)
            {
                var loopResult = PassHighway(route, speed, highwayResult.RouteTravelInfo);
                if (!loopResult.IsPassed)
                {
                    break;
                }

                highwayResult.Combine(loopResult);
            }
            
            return highwayResult;
        }

        private HighwayInfo PassHighway(
            IRoute route, 
            float speed, 
            RouteTravelInfo routeTravelInfo)
        {
            var currentHighway = route.Highways[routeTravelInfo.CurrentHighwayIndex];
            var distanceToHighwayEnd = GetDistanceToHighwayEnd(
                currentHighway,
                routeTravelInfo.CurrentPosition,
                routeTravelInfo.CurrentPointIndex);
            
            var timeToHighwayEnd = distanceToHighwayEnd / speed;
            var remainingTimeAfterHighway = routeTravelInfo.DeltaTime - timeToHighwayEnd;
            var isHighwayPassed = remainingTimeAfterHighway > 0 - float.Epsilon;

            if (!isHighwayPassed)
            {
                return new HighwayInfo();
            }
            
            var highways = new List<IHighway> { currentHighway };

            var nextHighwayIndex = GetNextIndex(route.Highways, routeTravelInfo.CurrentHighwayIndex);
            var newRouteTravelInfo = new RouteTravelInfo(
                nextHighwayIndex,
                0,
                route.Highways[nextHighwayIndex].Points[0].Position,
                remainingTimeAfterHighway);
            
            return new HighwayInfo(newRouteTravelInfo, highways);
        }
        
        private float GetRouteLength(IRoute route)
        {
            return route.Highways.Sum(GetHighwayLength);
        }

        private float GetHighwayLength(IHighway highway)
        {
            var length = 0f;
            var points = highway.Points;
            for (int i = 0; i < points.Count - 1; i++)
            {
                length += Vector3.Distance(points[i + 1].Position, points[i].Position);
            }
            
            return length;
        }
        
        private float GetDistanceToHighwayEnd(IHighway highway, Vector3 currentPosition, int lastPassedPointIndex)
        {
            var points = highway.Points;
            var nextPointIndex = GetNextIndex(points, lastPassedPointIndex);
            var distanceToNextPoint = GetDistanceToNextPoint(currentPosition, points[nextPointIndex]);
            if (nextPointIndex == 0)
            {
                return distanceToNextPoint;
            }
            
            for (int i = nextPointIndex; i < points.Count - 1; i++)
            {
                distanceToNextPoint += GetDistanceToNextPoint(points[i + 1], points[i]);
            }

            return distanceToNextPoint;
        }
        
        private float GetDistanceToNextPoint(Vector3 currentPosition, IPoint nextPoint)
        {
            var distanceToNextPoint = Vector3.Distance(nextPoint.Position, currentPosition); 
            return distanceToNextPoint;
        }
        
        private float GetDistanceToNextPoint(IPoint from, IPoint to)
        {
            var distanceToNextPoint = Vector3.Distance(to.Position, from.Position); 
            return distanceToNextPoint;
        }
        
        private int GetNextIndex<T>(IReadOnlyCollection<T> collection, int lastIndex)
        {
            var nextIndex = lastIndex + 1;
            if (nextIndex == collection.Count)
            {
                return 0;
            }

            return nextIndex;
        }
    }
}