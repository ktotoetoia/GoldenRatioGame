using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.SaveSystem
{
    public class AddressablePrefabResolver : IPrefabResolver
    {
        public GameObject ResolvePrefab(string prefabKey)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(prefabKey);
            return handle.WaitForCompletion(); 
        }
    }
}