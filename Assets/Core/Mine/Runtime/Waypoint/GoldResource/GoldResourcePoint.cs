using System.Collections.Generic;
using Common.Navigation.Runtime.Waypoint;
using Core.Job.Runtime;
using Core.Mine.Runtime.Waypoint;
using UnityEngine;

namespace Core.NavigationSystem.Runtime.Resource
{
    public class GoldResourcePoint : WaypointBase, IResourcePoint
    {
        [SerializeField] private GoldResourceDeposit[] m_ResourceDeposits;
        
        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
        public IList<IResourceDeposit> ResourceDeposits => m_ResourceDeposits;
    }
}