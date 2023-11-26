using System;
using System.Collections.Generic;

namespace Common.Moving.Runtime.Speed
{
    [Serializable]
    public class MovementSpeedConfig
    {
        public int StartSpeedIndex;
        public List<float> SpeedSettings;
    }
}