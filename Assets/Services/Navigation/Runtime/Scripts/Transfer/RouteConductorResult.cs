using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct RouteConductorResult
    {
        public int Index;
        public Vector3 Position;

        public RouteConductorResult(Vector3 position)
        {
            Index = 0;
            Position = position;
        }
        
        public RouteConductorResult(int index, Vector3 position)
        {
            Index = index;
            Position = position;
        }
    }
}