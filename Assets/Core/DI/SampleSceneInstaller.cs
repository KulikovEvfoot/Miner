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
    public class SampleSceneInstaller : MonoInstaller
    {
        [SerializeField] private SampleGameConfig m_SampleGameConfig;
        [SerializeField] private CanvasProvider m_CanvasProvider;
        
        public override void InstallBindings()
        {
            Debug.Log("Sample scene installer");
            
            //it's only to speed up the test.
            Container.Bind<SampleGameConfig>().FromScriptableObject(m_SampleGameConfig).AsSingle();
            Container.Bind<CanvasProvider>().FromInstance(m_CanvasProvider).AsSingle();
            
            //currency
            Container.Bind<GoldCurrencyController>().AsSingle().NonLazy();
            Container.Bind<IRewardCollector>().To<GoldCurrencyRewardCollector>().AsSingle().NonLazy();
            Container.Bind<RewardCollectorsController>().AsSingle().NonLazy();
            
            Container.Bind<AddressablesAssetLoader>().AsSingle().NonLazy();
            
            //core
            Container.Bind<ISpeedService>().To<MovementSpeedController>().AsSingle().NonLazy();
            Container.Bind<ResourcesExtractionController>().AsSingle().NonLazy();
            Container.Bind<MineManager>().AsSingle().NonLazy();
            
            Container.Bind<IInitPhase>().To<ResourcesExtractionController>().FromResolve().NonLazy();
            Container.Bind<IInitPhase>().To<MineManager>().FromResolve().NonLazy();
            Container.Bind<IInitPhase>().To<SampleViewController>().AsSingle().NonLazy();
            
            Container.Bind<IInitPhaseCompleteListener>().To<ResourcesExtractionController>().FromResolve().NonLazy();
            
            Container.Bind<IInitializable>().To<GameStarter>().AsSingle().NonLazy();
            
        }
    }
}