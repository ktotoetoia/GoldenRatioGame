using UnityEngine;

namespace IM.GoldenRatio
{
    public readonly struct RatioRegion
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public Vector2 Anchor { get; }
        public Bounds Bounds { get; }

        public RatioRegion(Vector2 start, Vector2 end, Vector2 anchor)
        {
            Start = start;
            End = end;
            Anchor = anchor;
            
            Bounds bounds = new Bounds();
            bounds.SetMinMax(
                new Vector3(Mathf.Min(start.x,end.x),Mathf.Min(start.y,end.y)), 
                new Vector3(Mathf.Max(start.x,end.x),Mathf.Max(start.y,end.y)));
            Bounds = bounds;
        }

        public bool TryGetDistance(out float distance, Vector2 position)
        {
            if (IsOnWrongSide(Anchor.x, Start.x, End.x, position.x) ||
                IsOnWrongSide(Anchor.y, Start.y, End.y, position.y))
            {
                distance = float.MaxValue;
                return false;
            }

            float arcRadius = Vector2.Distance(Anchor, Start);
            distance = Mathf.Abs(Vector2.Distance(position, Anchor) - arcRadius);
            
            return true;
        }
        
        private bool IsOnWrongSide(float anchorCoord, float startCoord, float endCoord, float posCoord)
        {
            if (!Mathf.Approximately(anchorCoord, startCoord) && !Mathf.Approximately(anchorCoord, endCoord))
            {
                return false;
            }

            return posCoord < Mathf.Min(startCoord, endCoord) || posCoord > Mathf.Max(startCoord, endCoord);
        }
    }
}