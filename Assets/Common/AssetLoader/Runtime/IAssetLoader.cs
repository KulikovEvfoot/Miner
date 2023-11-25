using UnityEngine;

namespace Common.AssetLoader.Runtime
{
    public interface IAssetLoader
    {
        Result<T> LoadSync<T>(string path) where T : Object;
    }
}