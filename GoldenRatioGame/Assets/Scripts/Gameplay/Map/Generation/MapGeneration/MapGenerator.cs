using IM.LifeCycle;
using UnityEngine;

namespace IM.Map.Grid
{
    public abstract class MapGenerator : ScriptableObject
    {
        public abstract void Create(IGameObjectFactory factory, int roomCount, int seed);
    }
}