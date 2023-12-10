using Common;
using UnityEngine;

namespace Services.AssetLoader.Runtime
{
    public class ResourceLoader : IAssetLoader
    {
        public Result<T> LoadSync<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"{nameof(ResourceLoader)} >> Path is null or empty");
                return new Result<T>(null, false);
            }
            
            var resource = Resources.Load<T>(path);
            if (resource == null)
            {
                Debug.LogError($"{nameof(ResourceLoader)} >> Can't load resource {typeof(T)} by path > {path}");
                return new Result<T>(null, false);
            }

            return new Result<T>(resource, true);
        }
    }
}