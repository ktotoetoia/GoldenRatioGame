using System;
using System.Collections.Generic;

namespace IM.LifeCycle
{
    public interface IEntityCollection : IReadOnlyCollection<IEntity>
    {
        event Action<IEntity> EntityAdded;
        event Action<IEntity> EntityRemoved;
        event Action<IEntity> EntityDestroyed;

        bool Add(IEntity entity);
        bool Remove(IEntity entity);
        bool Contains(IEntity entity);
        void Clear();
    }
}