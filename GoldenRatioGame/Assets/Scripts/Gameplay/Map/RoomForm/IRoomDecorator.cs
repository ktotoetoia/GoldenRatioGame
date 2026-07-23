using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public interface IRoomDecorator
    {
        IEnumerable<GameObject> Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory);
    }
}