using System.Collections.Generic;
using IM.Base;
using UnityEngine;

namespace IM.GoldenRatio
{
    public class GoldenRatioFactory : IFactory<RegionBasedGoldenRatio>, IFactory<RegionBasedGoldenRatio,int,int>
    {
        public Vector2 StartPosition { get; set; }
        public float Scale { get; set; }
        public int Count { get; set; }
        public int Rotation { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        
        public GoldenRatioFactory() : this(6,0)
        {
            
        }

        public GoldenRatioFactory(int count, int rotation)
        {
            Count = count;
            Rotation = rotation;
            Scale = 1f;
        }
        
        public RegionBasedGoldenRatio Create()
        {
            return Create(Count, Rotation);
        }

        public RegionBasedGoldenRatio Create(int count, int rotation)
        {
            List<RatioRegion> regions = new List<RatioRegion>();
            int[] fib = Fibonacci.GetSequence(count);
            Vector2 start = StartPosition;

            for (int i = 0; i < count; i++,rotation = (rotation + 1) % 4)
            {
                Vector2 end = start + GetDirection(rotation)* (Scale * fib[i] * GetFlip());
                Vector2 anchor = GetAnchor(start, end, rotation);
                
                regions.Add(new RatioRegion(start, end, anchor));

                start = end;
            }
            
            return new RegionBasedGoldenRatio(regions);
        }

        private Vector2 GetFlip()
        {
            return new Vector2(FlipX ? -1 : 1, FlipY ? -1 : 1);
        }

        private Vector2 GetAnchor(Vector2 start, Vector2 end, int rotation)
        {
            return rotation % 2 == 1 ?
                new Vector2(end.x, start.y) :
                new Vector2(start.x, end.y);
        }
        
        private Vector2 GetDirection(int rotation)
        {
            switch (rotation % 4)
            {
                case 0: return new Vector2(-1,1);
                case 1: return Vector2.one;
                case 2: return new Vector2(1,-1);
                case 3: return -Vector2.one;
                default: return Vector2.zero;
            }
        }
    }
}