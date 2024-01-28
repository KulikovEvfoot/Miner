using Services.Navigation.Runtime.Scripts.Configs;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts
{
    public interface ITransition
    {
        public IPoint From { get; }
        public IPoint To { get; }
        Vector3 GetTransitionDirection();
        float GetTransitionLength();
    }
}