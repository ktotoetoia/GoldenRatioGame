using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    /// <summary>
    /// A rule for WHERE a prop should go (never near ports, closer to center, only along the top
    /// row, etc.), completely independent of WHAT is being placed. A concrete decorator later pairs a
    /// strategy with an <see cref="IPropInfo"/> to actually spawn something.
    /// </summary>
    public interface IPropPlacementStrategy
    {
        /// <summary>
        /// Lazily yields candidate placements for a footprint of <paramref name="propSize"/> inside
        /// <paramref name="context"/>. Contract for implementers:
        /// - Each yielded placement must be free per context.GetAvailableTiles() at the moment it's
        ///   produced, so if the caller commits earlier placements via context.Add(...) mid-iteration,
        ///   later ones respect that.
        /// - The same start cell is never yielded twice within one enumeration.
        /// - When no valid candidate remains, the enumeration simply ends — no exception.
        /// </summary>
        IEnumerable<PropPlacementInfo> GetPlacements(RoomDecorationContext context, Vector2Int propSize);
    }
}