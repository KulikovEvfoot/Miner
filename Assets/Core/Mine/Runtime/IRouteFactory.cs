using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime
{
    public interface IRouteFactory
    {
        public IEnumerable<IPoint> Create { get; }
    }
}