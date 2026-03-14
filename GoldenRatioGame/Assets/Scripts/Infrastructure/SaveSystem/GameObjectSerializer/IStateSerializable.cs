using System;
using UnityEngine;

namespace IM.SaveSystem
{
    public interface IStateSerializable
    {
        GameObjectData Capture();
        void Restore(GameObjectData data, Func<string, GameObject> resolveDependency);
    }
}