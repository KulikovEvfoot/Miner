using System.Collections.Generic;
using Core.Currency.Runtime;
using Core.Currency.Runtime.Gold;
using Services.Navigation.Runtime.Scripts;
using UnityEngine;

namespace Core.Mine.Runtime.Waypoint.GoldResource
{
    public class GoldResourcePoint : WaypointBase, IResourcePoint
    {
        [SerializeField] private GoldReward m_GoldReward;
        [SerializeField] private GoldResourceView m_GoldResourceView;

        public override Vector3 Position => gameObject.transform.position;
        public IEnumerable<IResourceReward> ResourceRewards => new[] { m_GoldReward };
        public IEnumerable<IResourceReward> ExtractResources(int countOfLooping)
        {
            var result = new[] { new GoldReward(m_GoldReward.Value * countOfLooping) };
            return result;
        }
    }
}