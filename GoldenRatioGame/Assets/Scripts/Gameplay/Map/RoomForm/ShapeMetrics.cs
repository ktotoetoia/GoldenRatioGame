using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public readonly struct ShapeMetrics
    {
        public readonly int CellW, CellH;
        public readonly int TotalW, TotalH;
        public readonly int RoomOriginX, RoomOriginY;
        public readonly bool IsValid;
        
        private readonly int _minOffX, _minOffY;
        private readonly IRoomShape _shape;

        private ShapeMetrics(IRoomShape shape)
        {
            _shape   = shape;
            CellW    = (int)shape.CellRect.width;
            CellH    = (int)shape.CellRect.height;

            if (shape.Offsets == null || !shape.Offsets.Any())
            {
                _minOffX = _minOffY = TotalW = TotalH = RoomOriginX = RoomOriginY = 0;
                IsValid = false;
                return;
            }

            _minOffX = shape.Offsets.Min(o => o.x);
            _minOffY = shape.Offsets.Min(o => o.y);
            TotalW   = (shape.Offsets.Max(o => o.x) - _minOffX + 1) * CellW;
            TotalH   = (shape.Offsets.Max(o => o.y) - _minOffY + 1) * CellH;
            RoomOriginX = -TotalW / 2;
            RoomOriginY = -TotalH / 2;
            IsValid  = true;
        }

        public static ShapeMetrics From(IRoomShape shape) => new(shape);

        public (int startX, int startY) GetCellOrigin(Vector2Int offset) => (
            RoomOriginX + (offset.x - _minOffX) * CellW,
            RoomOriginY + (offset.y - _minOffY) * CellH);

        public bool IsInsideShape(Vector2 pos)
        {
            foreach (Vector2Int offset in _shape.Offsets)
            {
                var (startX, startY) = GetCellOrigin(offset);
                if (pos.x >= startX && pos.x < startX + CellW &&
                    pos.y >= startY && pos.y < startY + CellH)
                {
                    return true;
                }
            }
            return false;
        }

        public float GetEdgeDistance(Vector2 pos)
        {
            float minDist = float.MaxValue;

            foreach (Vector2Int offset in _shape.Offsets)
            {
                var (startX, startY) = GetCellOrigin(offset);
                float endX = startX + CellW;
                float endY = startY + CellH;

                if (!_shape.Offsets.Contains(new Vector2Int(offset.x - 1, offset.y))) minDist = Mathf.Min(minDist, pos.x - startX);
                if (!_shape.Offsets.Contains(new Vector2Int(offset.x + 1, offset.y))) minDist = Mathf.Min(minDist, endX - pos.x);
                if (!_shape.Offsets.Contains(new Vector2Int(offset.x, offset.y - 1))) minDist = Mathf.Min(minDist, pos.y - startY);
                if (!_shape.Offsets.Contains(new Vector2Int(offset.x, offset.y + 1))) minDist = Mathf.Min(minDist, endY - pos.y);
            }

            return Mathf.Max(0f, minDist);
        }
    }
}