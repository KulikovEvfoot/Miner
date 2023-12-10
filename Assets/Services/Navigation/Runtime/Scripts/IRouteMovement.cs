using System.Collections.Generic;
using Services.Navigation.Runtime.Scripts.Transfer;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IRouteMovement
    {
        void EnRouteMove(
            IEnumerable<ITransition> transitionsCollection,
            IRouteMoveListener routeMoveListener,
            TransferInfo transferInfo);
    }
}