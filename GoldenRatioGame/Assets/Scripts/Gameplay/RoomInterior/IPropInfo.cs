using UnityEngine;

namespace IM.Map
{
    /// <summary>
    /// Static identity of a placeable prop: what to spawn and how much space it needs.
    /// A concrete prop-group/config class will implement this per prefab (or prefab pool) and
    /// hand instances of it to a placement strategy.
    /// </summary>
    public interface IPropInfo
    {
        GameObject Prefab { get; }

        /// <summary>Footprint in grid cells, e.g. (1,1) or (2,2).</summary>
        Vector2Int CellSize { get; }

        /// <summary>
        /// Extra radius (world units) to keep clear around this prop's footprint once placed, on top
        /// of the footprint itself. Enforced centrally by <see cref="RoomDecorationContext"/> so every
        /// strategy honors it the same way, regardless of which strategy chose the spot.
        /// </summary>
        float ClearanceRadius { get; }
    }
}