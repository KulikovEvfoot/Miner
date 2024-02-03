using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IRouteMovement
    {
        void EnRouteMove(IEnumerable<IPoint> route);
    }
}