using UnityEngine;

namespace IM.LifeCycle
{
    public interface IParentRestorable
    {
        Transform DefaultParent { get; set; }
        void ResetToDefaultParent();
    }
}