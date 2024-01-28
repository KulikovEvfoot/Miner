using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Services.AssetLoader.Runtime
{
    public class AddressablesAssetLoader
    {
        public async Task<T> LoadAsync<T>(string path)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(path);
            return await asyncOperationHandle.Task;
        }
        
        public async Task<T> InstantiateAsync<T>(object address, InstantiationParameters instantiationParameters) 
            where T : class
        {
            var asyncOperationHandle = Addressables.InstantiateAsync(address,  instantiationParameters);
            var inst = await asyncOperationHandle.Task;
            return inst.TryGetComponent<T>(out var component) 
                ? component 
                : null;
        }
    }
}