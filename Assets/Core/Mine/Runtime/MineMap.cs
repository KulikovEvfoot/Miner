using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Mine.Runtime.Point;
using Core.Mine.Runtime.Point.Base;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime.Scripts;
using Services.Navigation.Runtime.Scripts.Configs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Core.Mine.Runtime
{
    public class MineMap : IInitPhase
    {
        [Inject] private AddressablesAssetLoader m_AssetLoader;
        [Inject] private SampleGameConfig m_SampleGameConfig;
        
        public Task Init()
        {
            return CreateLevel(m_SampleGameConfig.MineConfig);
        }

        private async Task CreateLevel(MineConfig mineConfig)
        {
            var tasks = new List<Task<Object>>();
            foreach (var point in mineConfig.Points)
            {
                if (point is IHasView hasView)
                {
                    var view = m_AssetLoader.InstantiateAsync<Object>(hasView.ViewAddress,
                        new InstantiationParameters(point.Position, quaternion.identity, null));
                    tasks.Add(view);
                }
            }

            await Task.WhenAll(tasks);
            
            
        }

        //for test
        public IBasePoint GetBasePoint()
        {
            var points = m_SampleGameConfig.MineConfig.Points;
            return (BasePoint)points.First(p => p.GetType() == typeof(BasePoint));
        }

        public List<ITransition> GetTransitions()
        {
            var points = m_SampleGameConfig.MineConfig.Points;

            var transitions = new List<ITransition>();
            transitions.Add(new Transition(points[0], points[1]));
            transitions.Add(new Transition(points[1], points[0]));
            
            return transitions;
        }
    }
}