using UnityEngine;

namespace IM.Map
{
    public abstract class MapGenerationStep : ScriptableObject
    {
        public abstract void Execute(MapGenerationContext context);
    }
}