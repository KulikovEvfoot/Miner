using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Configs
{
    public interface IPoint
    {
        int Id { get; }
        int[] NeighborsID { get; }
        Vector3 Position { get; }
    }
}