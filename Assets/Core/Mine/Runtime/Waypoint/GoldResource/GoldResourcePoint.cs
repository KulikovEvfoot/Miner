using System.Collections.Generic;
using Common.Navigation.Runtime.Transition;
using Common.Navigation.Runtime.Waypoint;
using Core.Job.Runtime;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.GoldResource
{
    public class GoldResourcePoint : WaypointBase, IResourcePoint
    {
        [SerializeField] private GoldResourceDeposit[] m_ResourceDeposits;
        [SerializeField] private Transition[] m_Transitions;
        [SerializeField] private GoldResourceView m_GoldResourceView;

        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
        public override IEnumerable<ITransition> Transitions => m_Transitions;
        
        public IEnumerable<IResourceDeposit> ResourceDeposits => m_ResourceDeposits;
        
        public void ResourceExtracted()
        {
            m_GoldResourceView.OnResourceExtracted();
        }
    }
}