using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public interface ITransitionTransfer
    {
        TransferInfo Transfer(
            float deltaTime,
            Vector3 currentPosition,
            ISpeedService speedService,
            ITransition transition);
    }
}