using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public interface IMovableBody
    {
        Transform Transform { get; }
    }
}