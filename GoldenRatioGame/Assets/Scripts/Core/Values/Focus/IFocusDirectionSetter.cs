using UnityEngine;

namespace IM.Values
{
    public interface IFocusDirectionSetter
    {
        void OverrideFocusDirection(Vector2 direction, float duration);
    }
}