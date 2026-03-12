using UnityEngine;

namespace IM.Abilities
{
    public interface IFocusPointProvider
    {
        float FocusTime { get; }
        Vector3 GetFocusPoint();
        Vector3 GetFocusDirection(); 
    }
}