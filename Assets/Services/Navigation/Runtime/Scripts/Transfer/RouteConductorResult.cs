using System.Collections.Generic;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorResult
    {
        public int LastPassedIndex { get; }
        public Vector3 CurrentPosition { get; }
        public IReadOnlyList<IPoint> PassedPoints { get; }
        public int PassedRoutesCount { get; }

        public RouteConductorResult(int lastPassedIndex, Vector3 currentPosition)
        {
            LastPassedIndex = lastPassedIndex;
            CurrentPosition = currentPosition;
            PassedPoints = default;
            PassedRoutesCount = default;
        }

        public RouteConductorResult(
            int lastPassedIndex,
            Vector3 currentPosition,
            IReadOnlyList<IPoint> passedPoints,
            int passedRoutesCount)
        {
            LastPassedIndex = lastPassedIndex;
            CurrentPosition = currentPosition;
            PassedPoints = passedPoints;
            PassedRoutesCount = passedRoutesCount;
        }
    }
}