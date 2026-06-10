using System;
using UnityEngine;

namespace IM.LifeCycle
{
    public interface IEntity
    {
        GameObject GameObject { get; }
        bool IsDestroyed { get; }
        void Destroy();
        event Action<IEntity> Destroyed;
    }

    public interface IEntityHitBox
    {
        IEntity Owner { get; }
    }
}