using Common;
using Core.Currency.Runtime.Gold;
using Core.Mine.Runtime;
using Core.ResourceExtraction;
using Services.AssetLoader.Runtime;
using Services.Currency.Runtime.Rewards;
using Services.Navigation.Runtime.Scripts.Transfer;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using Zenject;

namespace Core.DI
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private SampleGameConfig m_SampleGameConfig;
        
        public override void InstallBindings()
        {
            Debug.Log("INSTALL");
            
            //it's only to speed up the test.
            Container.Bind<SampleGameConfig>().FromScriptableObjectResource("SampleGameConfig").AsSingle().NonLazy();
            
            Container.Bind<GoldCurrencyController>().AsSingle().NonLazy();
            Container.Bind<IRewardCollector>().To<GoldCurrencyRewardCollector>().AsSingle().NonLazy();
            Container.Bind<RewardCollectorsController>().AsSingle().NonLazy();
            
            Container.Bind<AddressablesAssetLoader>().AsSingle().NonLazy();
            Container.Bind<ISpeedService>().To<MovementSpeedController>().AsSingle().NonLazy();
            
            //check
            Container.Bind<ResourcesExtractionController>().AsSingle().NonLazy();
            Container.Bind<IInitPhase>().To<ResourcesExtractionController>().FromResolve().NonLazy();
            
            Container.Bind<MineMap>().AsSingle().NonLazy();
            Container.Bind<IInitPhase>().To<MineMap>().FromResolve().NonLazy();
            
            Container.Bind<IInitPhase>().To<SampleViewController>().AsSingle().NonLazy();
            
            Container.Bind<IRouteConductor>().To<RouteConductor>().AsSingle().NonLazy();
            
            Container.Bind<IInitializable>().To<GameStarter>().AsSingle().NonLazy();
            
        }
    }
}