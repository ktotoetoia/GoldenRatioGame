using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    public class MultiCellPatternSelector : IPatternSelector
    {
        public void SelectMatchingRoomPatterns(IGrid<ICellInfo> grid, int seed)
        {
            Random random = new(seed);
            int nextRoomInstanceId = 0;
            HashSet<Vector2Int> unassigned = new(grid.OccupiedPositions());

            while (unassigned.Count > 0)
            {
                Vector2Int anchor = unassigned.First();
                ICellInfo anchorCell = grid[anchor];

                var region = FloodFill(grid, anchor, anchorCell.Factory, unassigned);
                var candidates = FindCandidates(grid, region, anchorCell.Factory);

                if (candidates.Count == 0)
                {
                    Debug.LogWarning($"No candidates found for region starting at {anchor}");
                    unassigned.Remove(anchor);
                    continue;
                }

                var (pattern, rootPosition) = candidates[random.Next(candidates.Count)];
                int roomInstanceId = nextRoomInstanceId++;

                foreach (Vector2Int offset in pattern.Shape.Offsets)
                {
                    Vector2Int worldPos = rootPosition + offset;
                    ICellInfo cell = grid[worldPos];
                    cell.Pattern = pattern;
                    cell.Offset = offset;
                    cell.RoomInstanceId = roomInstanceId;
                    unassigned.Remove(worldPos);
                }
            }

            SelectAll(grid);
        }

        private HashSet<Vector2Int> FloodFill(IGrid<ICellInfo> grid, Vector2Int start, IRoomFactory factory, HashSet<Vector2Int> unassigned)
        {
            HashSet<Vector2Int> visited = new();
            Queue<Vector2Int> queue = new();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                if (!visited.Add(current)) continue;

                foreach (Vector2Int dir in PortSideUtility.Directions)
                {
                    Vector2Int neighbor = current + dir;
                    if (unassigned.Contains(neighbor) && grid.IsOccupied(neighbor) && grid[neighbor].Factory == factory)
                        queue.Enqueue(neighbor);
                }
            }
            return visited;
        }

        private List<(IRoomPattern Pattern, Vector2Int Root)> FindCandidates(IGrid<ICellInfo> grid, HashSet<Vector2Int> region, IRoomFactory factory)
        {
            List<(IRoomPattern, Vector2Int)> candidates = new();
            foreach (IRoomPattern pattern in factory.GetRoomPatterns())
                foreach (Vector2Int root in region)
                    if (CanPlacePattern(grid, pattern, root, region))
                        candidates.Add((pattern, root));
            return candidates;
        }

        private bool CanPlacePattern(IGrid<ICellInfo> grid, IRoomPattern pattern, Vector2Int root, HashSet<Vector2Int> region)
        {
            if (!pattern.Shape.Offsets.All(offset => region.Contains(root + offset))) return false;

            foreach (Vector2Int localOffset in pattern.Shape.Offsets)
            {
                Vector2Int worldPos = root + localOffset;
                pattern.RequiredPortDefinitions.TryGetValue(localOffset, out var req);
                pattern.OptionalPortDefinitions.TryGetValue(localOffset, out var opt);

                foreach (Vector2Int dir in PortSideUtility.Directions)
                {
                    if (pattern.Shape.Offsets.Contains(localOffset + dir)) continue;

                    Vector2Int neighborWorld = worldPos + dir;
                    PortSide side = PortSideUtility.FromDirection(dir);
                    bool hasNeighbor = grid.IsOccupied(neighborWorld);
                    bool hasPort = (req?.Any(p => p.Side == side) ?? false) || (opt?.Any(p => p.Side == side) ?? false);

                    if ((req?.Any(p => p.Side == side) ?? false) && !hasNeighbor) return false;
                    if (hasNeighbor && !hasPort) return false;
                    if (hasNeighbor && grid[neighborWorld] is { Pattern: not null } n && !HasPort(n.Pattern, n.Offset, PortSideUtility.Opposite(side))) return false;
                }
            }
            return true;
        }

        private void SelectAll(IGrid<ICellInfo> grid)
        {
            var roomGroups = grid.OccupiedPositions()
                .Select(pos => grid[pos])
                .Where(cell => cell.Pattern != null)
                .GroupBy(cell => cell.RoomInstanceId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var selectedOptionalsByRoom = roomGroups.Keys.ToDictionary(
                id => id, _ => new Dictionary<Vector2Int, HashSet<IPortDefinition>>());

            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                ICellInfo cell = grid[pos];
                if (cell.Pattern == null) continue;

                foreach (Vector2Int dir in PortSideUtility.Directions)
                {
                    Vector2Int neighborPos = pos + dir;
                    if (pos.x > neighborPos.x || (pos.x == neighborPos.x && pos.y > neighborPos.y)) continue;
                    if (!grid.IsOccupied(neighborPos) || grid[neighborPos] is not { Pattern: not null } neighbor || neighbor.RoomInstanceId == cell.RoomInstanceId) continue;

                    PortSide side = PortSideUtility.FromDirection(dir);
                    PortSide opposite = PortSideUtility.Opposite(side);

                    bool cellCanConnect = HasPort(cell.Pattern, cell.Offset, side);
                    bool neighborCanConnect = HasPort(neighbor.Pattern, neighbor.Offset, opposite);

                    if (HasPort(cell.Pattern, cell.Offset, side, true) && !neighborCanConnect)
                        throw new InvalidOperationException($"Required port mismatch: room {cell.RoomInstanceId} at {pos} facing {side} missing counterpart.");
                    if (HasPort(neighbor.Pattern, neighbor.Offset, opposite, true) && !cellCanConnect)
                        throw new InvalidOperationException($"Required port mismatch: room {neighbor.RoomInstanceId} at {neighborPos} facing {opposite} missing counterpart.");

                    if (!cellCanConnect || !neighborCanConnect) continue;

                    AddSelectedPort(selectedOptionalsByRoom[cell.RoomInstanceId], cell.Offset, cell.Pattern, side);
                    AddSelectedPort(selectedOptionalsByRoom[neighbor.RoomInstanceId], neighbor.Offset, neighbor.Pattern, opposite);
                }
            }

            foreach (var (id, cells) in roomGroups)
            {
                var optionals = selectedOptionalsByRoom[id].ToDictionary(kvp => kvp.Key, kvp => (IEnumerable<IPortDefinition>)kvp.Value);
                ISelectedRoomPattern selectedPattern = cells[0].Pattern.Select(optionals);

                foreach (ICellInfo cell in cells)
                    cell.SelectedPattern = selectedPattern;
            }
        }

        private void AddSelectedPort(Dictionary<Vector2Int, HashSet<IPortDefinition>> roomPorts, Vector2Int offset, IRoomPattern pattern, PortSide side)
        {
            if (!pattern.OptionalPortDefinitions.TryGetValue(offset, out var optional)) return;

            foreach (IPortDefinition port in optional.Where(p => p.Side == side))
            {
                if (!roomPorts.TryGetValue(offset, out var set))
                    roomPorts[offset] = set = new();
                set.Add(port);
            }
        }

        private bool HasPort(IRoomPattern pattern, Vector2Int offset, PortSide side, bool requiredOnly = false) =>
            (pattern.RequiredPortDefinitions.TryGetValue(offset, out var req) && req.Any(p => p.Side == side)) ||
            (!requiredOnly && pattern.OptionalPortDefinitions.TryGetValue(offset, out var opt) && opt.Any(p => p.Side == side));
    }
}