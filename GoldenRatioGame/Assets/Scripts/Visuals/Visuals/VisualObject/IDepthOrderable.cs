using UnityEngine;

namespace IM.Visuals
{
    public interface IDepthOrderable : IOrderable
    {
        /// <summary>Where the object touches the ground. Not affected by Elevation.</summary>
        Vector3 ReferencePoint { get; }

        /// <summary>Bottom of the vertical span, in the same units as ReferencePoint.y.</summary>
        float Elevation { get; }

        /// <summary>Vertical extent. Top == Elevation + Height.</summary>
        float Height { get; }

        /// <summary>Horizontal half-extent, used to skip objects that can't visually overlap.</summary>
        float HalfWidth { get; }
    }
}