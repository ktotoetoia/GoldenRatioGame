using System;
using UnityEngine;

namespace IM.SaveSystem
{
    public interface IStateSerializable
    {
        int Order { get;}
        GameObjectData Capture();
        void Restore(GameObjectData data, Func<string, GameObject> resolveDependency);
    }
}