using UnityEngine;

namespace IM.Transforms
{
    public interface IHaveRotation
    {
        Quaternion Rotation { get; }
        Quaternion LocalRotation {get;}
    }
}