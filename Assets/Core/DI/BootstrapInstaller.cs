using UnityEngine;
using Zenject;

namespace Core.DI
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log("Global installer");
        }
    }
}