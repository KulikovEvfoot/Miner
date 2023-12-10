using System;
using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts.Transfer.Speed
{
    [Serializable]
    public class SpeedConfig
    {
        public int StartSpeedIndex;
        public List<float> SpeedSettings;

        public SpeedConfig(int startSpeedIndex, List<float> speedSettings)
        {
            StartSpeedIndex = startSpeedIndex;
            SpeedSettings = speedSettings;
        }
    }
}