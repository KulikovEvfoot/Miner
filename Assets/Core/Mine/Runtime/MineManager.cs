using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Mine.Runtime.Point;
using Core.Mine.Runtime.Point.Base;
using Services.AssetLoader.Runtime;
using Services.Navigation.Runtime.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Core.Mine.Runtime
{
    //rename
    public class MineManager : IInitPhase
    {
        [Inject] private AddressablesAssetLoader m_AssetLoader;
        [Inject] private SampleGameConfig m_SampleGameConfig;
        
        public Task Init()
        {
            return CreateLevel(m_SampleGameConfig.MineConfig);
        }

        private async Task CreateLevel(IMineConfig mineConfig)
        {
            var tasks = new List<Task<Object>>();
            var navigationMap = mineConfig.GetMineMap().NavigationMap;
            foreach (var pair in navigationMap)
            {
                var point = pair.Key;
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
            var navigationMap
                = m_SampleGameConfig.MineConfig.GetMineMap().NavigationMap;
            
            var basePoint = navigationMap.Keys.FirstOrDefault(p => p.GetType() == typeof(BasePoint)) as IBasePoint;
            if (basePoint == null)
            {
                Debug.LogError("Can't find base point");
                return null;
            }
            
            return basePoint;
        }

        public IRoute CreateRoute()
        {
            var mineConfig = m_SampleGameConfig.MineConfig;
            var routeFactory = new RouteFactory(mineConfig);
            var transitions = routeFactory.Create();
            return transitions;
        } 
    }
}