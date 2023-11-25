using Common.Moving.Runtime.Speed;

namespace Common.Moving.Runtime
{
    public class UnitMovementFactory : IUnitMovementFactory
    {
        private readonly IMovementSpeedService m_MovementSpeedService;
        private readonly ICoroutineRunner m_CoroutineRunner;

        public UnitMovementFactory(
            IMovementSpeedService movementSpeedService,
            ICoroutineRunner coroutineRunner)
        {
            m_MovementSpeedService = movementSpeedService;
            m_CoroutineRunner = coroutineRunner;
        }

        public IUnitMovement Create()
        {
            return new UnitMovement(m_MovementSpeedService, m_CoroutineRunner);
        }
    }
}