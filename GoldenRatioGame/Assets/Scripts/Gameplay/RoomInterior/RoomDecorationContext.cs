using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    /// <summary>
    /// Owns a single room's decoration state: the shape, which cells are still free, and the props
    /// already committed. This is the only thing allowed to mutate tile availability, so every
    /// placement strategy — no matter when it runs — sees the same live picture.
    /// </summary>
    public sealed class RoomDecorationContext
    {
        private readonly HashSet<Vector2Int> _availableCells;
        private readonly List<PropPlacementInfo> _placements = new();

        public IRoomShape RoomShape { get; }
        public ShapeMetrics Metrics => RoomShape.Metrics;
        public float CellStep { get; }
        public IReadOnlyList<PropPlacementInfo> Placements => _placements;

        public RoomDecorationContext(IRoomShape roomShape, float cellStep,
            IEnumerable<Vector2> portExclusionPositions = null, float portClearance = 0f)
        {
            RoomShape = roomShape ?? throw new ArgumentNullException(nameof(roomShape));
            if (cellStep <= 0f) throw new ArgumentOutOfRangeException(nameof(cellStep), "Cell step must be positive.");

            CellStep = cellStep;
            _availableCells = BuildInitialGrid(portExclusionPositions, portClearance);
        }

        private HashSet<Vector2Int> BuildInitialGrid(IEnumerable<Vector2> portExclusionPositions, float portClearance)
        {
            var grid = new HashSet<Vector2Int>();
            var ports = portExclusionPositions?.ToList() ?? new List<Vector2>();
            
            int gridWidth = Mathf.CeilToInt(Metrics.TotalW / CellStep);
            int gridHeight = Mathf.CeilToInt(Metrics.TotalH / CellStep);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var cell = new Vector2Int(x, y);
                    Vector2 center = CellToWorld(cell);

                    if (!Metrics.IsInsideShape(center)) continue;
                    if (portClearance > 0f && ports.Any(p => Vector2.Distance(center, p) < portClearance)) continue;

                    grid.Add(cell);
                }
            }

            return grid;
        }

        public Vector2 CellToWorld(Vector2Int cell)
        {
            return new Vector2(
                Metrics.RoomOriginX + (cell.x + 0.5f) * CellStep,
                Metrics.RoomOriginY + (cell.y + 0.5f) * CellStep);
        }

        public Vector2 GetPlacementWorldCenter(PropPlacementInfo placement)
        {
            Vector2Int size = placement.Size;
            return new Vector2(
                Metrics.RoomOriginX + (placement.StartCell.x + size.x * 0.5f) * CellStep,
                Metrics.RoomOriginY + (placement.StartCell.y + size.y * 0.5f) * CellStep);
        }

        /// <summary>
        /// Cells not yet claimed by a footprint or clearance radius. This is a live view — re-enumerate
        /// after every <see cref="Add"/> rather than caching the result.
        /// </summary>
        public IEnumerable<Vector2Int> GetAvailableTiles() => _availableCells;

        public bool IsAvailable(Vector2Int cell) => _availableCells.Contains(cell);

        /// <summary>True if every cell of the placement is currently free and the whole footprint sits inside the room shape.</summary>
        public bool CanPlace(PropPlacementInfo placement)
        {
            foreach (Vector2Int cell in placement.EnumerateCells())
            {
                if (!_availableCells.Contains(cell)) return false;
            }
            return IsFootprintInsideShape(placement);
        }

        private bool IsFootprintInsideShape(PropPlacementInfo placement)
        {
            Vector2 center = GetPlacementWorldCenter(placement);
            Vector2Int size = placement.Size;
            float halfW = size.x * CellStep * 0.5f;
            float halfH = size.y * CellStep * 0.5f;

            return Metrics.IsInsideShape(center + new Vector2(halfW, halfH)) &&
                   Metrics.IsInsideShape(center + new Vector2(-halfW, halfH)) &&
                   Metrics.IsInsideShape(center + new Vector2(halfW, -halfH)) &&
                   Metrics.IsInsideShape(center + new Vector2(-halfW, -halfH));
        }

        /// <summary>
        /// Commits a placement: removes its footprint (and, if given, a clearance radius around its
        /// center) from the available cells, and records it. Throws if the placement isn't currently
        /// valid — callers should check <see cref="CanPlace"/> first (strategies built on
        /// <see cref="PropPlacementStrategyBase"/> already guarantee this).
        /// </summary>
        public PropPlacementInfo Add(PropPlacementInfo placement, float clearanceRadius = 0f)
        {
            if (!CanPlace(placement))
                throw new InvalidOperationException(
                    $"Cannot commit placement {placement.StartCell}-{placement.EndCell}: tiles are unavailable or outside the room shape.");

            foreach (Vector2Int cell in placement.EnumerateCells())
            {
                _availableCells.Remove(cell);
            }

            if (clearanceRadius > 0f)
            {
                Vector2 center = GetPlacementWorldCenter(placement);
                _availableCells.RemoveWhere(cell => Vector2.Distance(CellToWorld(cell), center) < clearanceRadius);
            }

            _placements.Add(placement);
            return placement;
        }
    }
}