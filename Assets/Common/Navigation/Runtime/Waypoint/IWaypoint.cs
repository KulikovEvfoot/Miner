using System.Collections.Generic;
using Common.Navigation.Runtime.Transition;
using UnityEngine;

namespace Common.Navigation.Runtime.Waypoint
{
    public interface IWaypoint
    {
        Transform Transform { get; }
        Vector3 Position { get; }
        IEnumerable<ITransition> Transitions { get; }
    }
}