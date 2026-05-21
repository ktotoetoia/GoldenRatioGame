using IM.LifeCycle;
using UnityEngine;

namespace IM.Map.Grid
{
    public abstract class MapFactory : ScriptableObject,IMapFactory
    {
        public abstract IMapInfo Create(IGameObjectFactory factory, int seed, int depth);
    }
}