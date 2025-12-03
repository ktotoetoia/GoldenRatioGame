using UnityEngine;

namespace IM.Visuals
{
    public interface IHaveRotation
    {
        Quaternion Rotation { get; }
        Quaternion LocalRotation {get;}
    }
}