using System;
using UnityEngine;

namespace IM.LifeCycle
{
    public interface IEntity
    {
        GameObject GameObject { get; }
        void Destroy();
        event Action<IEntity> Destroyed;
    }
}