using System.Collections.Generic;
using UnityEngine;

namespace Core.Mine.Runtime
{
    [System.Serializable]
    public class HighwayIndexes
    {
        [SerializeField] private List<int> m_PathIndexes;

        public List<int> PathIndexes => m_PathIndexes;
    }
}