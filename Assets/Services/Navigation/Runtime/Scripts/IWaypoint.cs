using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IWaypoint
    {
        Vector3 Position { get; }
    }
}