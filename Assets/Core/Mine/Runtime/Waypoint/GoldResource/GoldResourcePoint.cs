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

        public override Transform Transform => gameObject.transform;
        public override Vector3 Position => gameObject.transform.position;
        public override IEnumerable<ITransition> Transitions => m_Transitions;
        public IList<IResourceDeposit> ResourceDeposits => m_ResourceDeposits;
    }
}