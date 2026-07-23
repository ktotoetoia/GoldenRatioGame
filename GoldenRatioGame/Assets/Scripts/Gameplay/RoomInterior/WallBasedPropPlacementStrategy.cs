using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    [Serializable]
    public class WallBasedPropPlacementStrategy : PropPlacementStrategyBase
    {
        [SerializeField] private bool _inverse;
        [SerializeField] private WallProperties _left = new();
        [SerializeField] private WallProperties _right = new();
        [SerializeField] private WallProperties _top = new();
        [SerializeField] private WallProperties _bottom = new();

        public bool Inverse { get => _inverse; set => _inverse = value; }
        public WallProperties Left { get => _left; set => _left = value; }
        public WallProperties Right { get => _right; set => _right = value; }
        public WallProperties Top { get => _top; set => _top = value; }
        public WallProperties Bottom { get => _bottom; set => _bottom = value; }

        private RoomDecorationContext _cachedContext;
        private Dictionary<int, (int minY, int maxY)> _columnBounds;
        private Dictionary<int, (int minX, int maxX)> _rowBounds;

        public WallBasedPropPlacementStrategy() { }

        public WallBasedPropPlacementStrategy(
            WallProperties top = null,
            WallProperties bottom = null,
            WallProperties left = null,
            WallProperties right = null,
            bool inverse = false)
        {
            _top = top ?? new WallProperties();
            _bottom = bottom ?? new WallProperties();
            _left = left ?? new WallProperties();
            _right = right ?? new WallProperties();
            _inverse = inverse;
        }

        protected override bool IsCandidateValid(Candidate candidate, RoomDecorationContext context, Vector2Int propSize)
        {
            bool anyWallEnabled = _top is { Place: true } || _bottom is { Place: true } ||_left is { Place: true } ||_right is { Place: true };
            if (!anyWallEnabled) return false;

            EnsureBoundsCached(context);

            var placement = PropPlacementInfo.FromOrigin(candidate.BottomLeftCell, propSize);
            bool isInWallArea = IsPlacementInWallArea(placement);

            return _inverse ? !isInWallArea : isInWallArea;
        }

        private bool IsPlacementInWallArea(PropPlacementInfo placement)
        {
            Vector2Int start = placement.StartCell;
            Vector2Int end = placement.EndCell;

            if (_top is { Place: true })
            {
                bool topValid = true;
                for (int x = start.x; x <= end.x; x++)
                {
                    if (!_columnBounds.TryGetValue(x, out var col)) { topValid = false; break; }
                    float dist = col.maxY - end.y;
                    if (dist < _top.MinDistance || dist > _top.MaxDistance)
                    {
                        topValid = false;
                        break;
                    }
                }
                if (topValid) return true;
            }

            if (_bottom is { Place: true })
            {
                bool bottomValid = true;
                for (int x = start.x; x <= end.x; x++)
                {
                    if (!_columnBounds.TryGetValue(x, out var col)) { bottomValid = false; break; }
                    float dist = start.y - col.minY;
                    if (dist < _bottom.MinDistance || dist > _bottom.MaxDistance)
                    {
                        bottomValid = false;
                        break;
                    }
                }
                if (bottomValid) return true;
            }

            if (_left is { Place: true })
            {
                bool leftValid = true;
                for (int y = start.y; y <= end.y; y++)
                {
                    if (!_rowBounds.TryGetValue(y, out var row)) { leftValid = false; break; }
                    float dist = start.x - row.minX;
                    if (dist < _left.MinDistance || dist > _left.MaxDistance)
                    {
                        leftValid = false;
                        break;
                    }
                }
                if (leftValid) return true;
            }

            if (_right is { Place: true })
            {
                bool rightValid = true;
                for (int y = start.y; y <= end.y; y++)
                {
                    if (!_rowBounds.TryGetValue(y, out var row)) { rightValid = false; break; }
                    float dist = row.maxX - end.x;
                    if (dist < _right.MinDistance || dist > _right.MaxDistance)
                    {
                        rightValid = false;
                        break;
                    }
                }
                if (rightValid) return true;
            }

            return false;
        }

        private void EnsureBoundsCached(RoomDecorationContext context)
        {
            if (_cachedContext == context && _columnBounds != null && _rowBounds != null)
                return;

            _cachedContext = context;
            _columnBounds = new Dictionary<int, (int minY, int maxY)>();
            _rowBounds = new Dictionary<int, (int minX, int maxX)>();

            int gridWidth = Mathf.CeilToInt(context.Metrics.TotalW / context.CellStep);
            int gridHeight = Mathf.CeilToInt(context.Metrics.TotalH / context.CellStep);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var cell = new Vector2Int(x, y);
                    if (context.Metrics.IsInsideShape(context.CellToWorld(cell)))
                    {
                        if (!_columnBounds.TryGetValue(x, out var col))
                            _columnBounds[x] = (y, y);
                        else
                            _columnBounds[x] = (Math.Min(col.minY, y), Math.Max(col.maxY, y));

                        if (!_rowBounds.TryGetValue(y, out var row))
                            _rowBounds[y] = (x, x);
                        else
                            _rowBounds[y] = (Math.Min(row.minX, x), Math.Max(row.maxX, x));
                    }
                }
            }
        }

        [Serializable]
        public class WallProperties
        {
            public bool Place = true;
            public float MinDistance = 0;
            public float MaxDistance = 3;
        }
    }
}