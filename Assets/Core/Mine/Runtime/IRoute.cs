using System.Collections.Generic;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public interface IRoute
    {
        public IEnumerable<ITransition> Transitions { get; }
    }
}