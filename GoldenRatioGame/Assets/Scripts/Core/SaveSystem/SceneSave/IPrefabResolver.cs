using UnityEngine;

namespace IM.SaveSystem
{
    public interface IPrefabResolver
    {
        GameObject ResolvePrefab(string prefabKey);
    }
}