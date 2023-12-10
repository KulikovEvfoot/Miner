using UnityEngine;

namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public struct TransferInfo
    {
        public Vector3 Position { get; }
        public float Time { get; }
        public bool IsTransferComplete { get; }

        public TransferInfo(Vector3 position, float time, bool isTransferComplete)
        {
            Position = position;
            Time = time;
            IsTransferComplete = isTransferComplete;
        }
        
        public TransferInfo(Vector3 position, float time)
        {
            Position = position;
            Time = time;
            IsTransferComplete = false;
        }

        public TransferInfo(Vector3 position)
        {
            Position = position;
            Time = 0;
            IsTransferComplete = false;
        }
    }
}