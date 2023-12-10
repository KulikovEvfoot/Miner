using Common;
using UnityEngine;

namespace Services.AssetLoader.Runtime
{
    public interface IAssetLoader
    {
        Result<T> LoadSync<T>(string path) where T : Object;
    }
}