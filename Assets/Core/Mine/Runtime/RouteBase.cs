using System.Collections.Generic;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime
{
    public abstract class RouteBase : MonoBehaviour, IRoute
    {
        public abstract IEnumerable<ITransition> Transitions { get; }
    }
}