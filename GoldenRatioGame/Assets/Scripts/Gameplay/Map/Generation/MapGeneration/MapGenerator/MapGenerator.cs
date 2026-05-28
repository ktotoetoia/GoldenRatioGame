using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Map Generator")]
    public class MapGenerator : ScriptableObject
    {
        [SerializeField] private List<MapGenerationStep> _steps;
        
        public MapGenerationContext Generate(int seed, int depth)
        {
            MapGenerationContext context = new MapGenerationContext(seed, depth);

            foreach (MapGenerationStep step in _steps)
            {
                step.Execute(context);
            }
            
            return context;
        }
    }
}