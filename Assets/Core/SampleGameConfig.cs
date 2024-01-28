using Core.Mine.Runtime;
using Services.Navigation.Runtime.Scripts.Transfer.Speed;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    [CreateAssetMenu(fileName = "SampleGameConfig", menuName = "Config/SampleGameConfig", order = 0)]
    public class SampleGameConfig : ScriptableObject
    {
        [SerializeField] private AssetReference m_MinerBody;
        [SerializeField] private AssetReference m_SampleViewReference;
        [SerializeField] private MineConfig m_MineConfig;
        [SerializeField] private SamplePriceConfig m_SamplePriceConfig;
        [SerializeField] private SpeedConfig m_SpeedConfig;
        
        public AssetReference SampleViewReference => m_SampleViewReference;
        public SpeedConfig SpeedConfig => m_SpeedConfig;
        public AssetReference MinerBody => m_MinerBody;
        public MineConfig MineConfig => m_MineConfig;
        public SamplePriceConfig SamplePriceConfig => m_SamplePriceConfig;
        
        
    }
}