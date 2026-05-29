using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "2D/Tiles/Custom Rule Tile")]
    public class CustomRuleTile : RuleTile
    {
        public override bool RuleMatch(int neighbor, TileBase other) {
            switch (neighbor) 
            {
                case TilingRuleOutput.Neighbor.This:
                    return other != null;
                case TilingRuleOutput.Neighbor.NotThis:
                    return other == null;
            }
        
            return base.RuleMatch(neighbor, other);
        }
    }
}