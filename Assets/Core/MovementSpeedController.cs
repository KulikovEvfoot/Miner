using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using Zenject;

namespace Core
{
    public class MovementSpeedController : ISpeedService
    {
        private readonly MovementSpeedService m_MovementSpeedService;

        [Inject]
        public MovementSpeedController(SampleGameConfig sampleGameConfig)
        {
            m_MovementSpeedService = new MovementSpeedService(sampleGameConfig.SpeedConfig);
        }

        public float Speed => m_MovementSpeedService.Speed;
        public void Inc()
        {
            m_MovementSpeedService.Inc();
        }

        public void Dec()
        {
            m_MovementSpeedService.Dec();
        }

        public bool CanInc()
        {
            return m_MovementSpeedService.CanInc();
        }

        public bool CanDec()
        {
            return m_MovementSpeedService.CanDec();
        }
    }
}