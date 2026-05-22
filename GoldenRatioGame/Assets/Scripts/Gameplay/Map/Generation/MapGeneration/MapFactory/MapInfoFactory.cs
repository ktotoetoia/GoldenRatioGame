using IM.LifeCycle;
using UnityEngine;

namespace IM.Map.Grid
{
    public abstract class MapInfoFactory : ScriptableObject, IMapInfoFactory
    {
        [field:SerializeField] public string AddresableAddress { get; protected set; }
        public abstract IMapInfo Create(IGameObjectFactory factory, int seed, int depth);
    }
}