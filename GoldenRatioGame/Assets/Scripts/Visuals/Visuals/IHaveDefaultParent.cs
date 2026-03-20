using UnityEngine;

namespace IM.Visuals
{
    public interface IHaveDefaultParent
    {
        Transform DefaultParent { get; set; }
    }
}