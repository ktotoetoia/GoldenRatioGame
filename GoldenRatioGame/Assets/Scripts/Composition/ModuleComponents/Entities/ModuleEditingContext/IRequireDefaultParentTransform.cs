using UnityEngine;

namespace IM
{
    public interface IRequireDefaultParentTransform
    {
        Transform DefaultParentTransform { get; set; }
    }
}