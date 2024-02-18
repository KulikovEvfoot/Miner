using System.Collections.Generic;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.Defined
{
    public class DefinedRouteBuildType : IRouteBuildType
    {
        [SerializeField] private List<HighwayIndexes> m_Highways;
        
        public List<HighwayIndexes> Highways => m_Highways;
    }
}