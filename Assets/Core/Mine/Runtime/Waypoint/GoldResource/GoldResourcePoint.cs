using System.Collections.Generic;
using Core.Currency.Runtime.Gold;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.GoldResource
{
    public class GoldResourcePoint : WaypointBase, IResourcePoint
    {
        [SerializeField] private GoldReward m_Reward;
        [SerializeField] private GoldResourceView m_GoldResourceView;

        public override Vector3 Position => gameObject.transform.position;

        public IEnumerable<IReward> Rewards => new[] { m_Reward };
    }
}