using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Generation Steps/Cell Pattern Selection Step")]
    public class CellPatternSelectionStep : MapGenerationStep
    {
        [SerializeField] private bool _isSingle;
        
        public override void Execute(MapGenerationContext context)
        {
            if (_isSingle)
            {
                new SingleCellPatternSelector().SelectMatchingRoomPatterns(context.Grid,context.Seed);
                
                return;
            }
            
            new MultiCellPatternSelector().SelectMatchingRoomPatterns(context.Grid, context.Seed);
        }
    }
}