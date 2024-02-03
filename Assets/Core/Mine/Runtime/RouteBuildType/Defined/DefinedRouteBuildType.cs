using System.Collections.Generic;
using UnityEngine;

namespace Core.Mine.Runtime.RouteBuildType.Defined
{
    public class DefinedRouteBuildType : IRouteBuildType
    {
        [SerializeField] private List<int> m_NavigationPathIndexes;
        public IReadOnlyList<int> NavigationPathIndexes => m_NavigationPathIndexes;
    }
}