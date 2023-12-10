using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts.Transfer.Speed
{
    public class MovementSpeedService : ISpeedService
    {
        private readonly List<float> m_SpeedSetting;
        private int m_SpeedIndex;
        private float m_Speed;
        
        public float Speed => m_Speed;

        public MovementSpeedService(SpeedConfig config)
        {
            m_SpeedSetting = config.SpeedSettings;
            m_SpeedIndex = config.StartSpeedIndex;
            if (m_SpeedIndex >= m_SpeedSetting.Count)
            {
                m_Speed = m_SpeedSetting[m_SpeedSetting.Count - 1];
                return;
            }
            
            m_Speed = m_SpeedSetting[m_SpeedIndex];
        }

        public void Inc()
        {
            var maxSpeedIndex = m_SpeedSetting.Count - 1;
            if (m_SpeedIndex >= maxSpeedIndex)
            {
                m_Speed = m_SpeedSetting[maxSpeedIndex];
                return;
            }

            m_SpeedIndex++;
            m_Speed = m_SpeedSetting[m_SpeedIndex];
        }

        public void Dec()
        {
            if (m_SpeedIndex <= 0)
            {
                m_Speed = m_SpeedSetting[0];
                return;
            }

            m_SpeedIndex--;
            m_Speed = m_SpeedSetting[m_SpeedIndex];
        }

        public bool CanInc()
        {
            var maxSpeedIndex = m_SpeedSetting.Count - 1;
            return m_SpeedIndex < maxSpeedIndex;
        }

        public bool CanDec()
        {
            return m_SpeedIndex > 0;
        }
    }
}