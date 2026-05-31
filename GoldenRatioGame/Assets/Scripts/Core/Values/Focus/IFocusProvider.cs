using UnityEngine;

namespace IM.Abilities
{
    public interface IFocusProvider
    {
        float FocusTime { get; }
        Vector3 GetFocusPoint();
        Vector3 GetFocusDirection(); 
    }
}