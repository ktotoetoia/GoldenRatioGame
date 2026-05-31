using UnityEngine;

namespace IM.Values
{
    public interface IFocusDirectionOverrider
    {
        void OverrideFocusDirection(Vector2 direction, float duration);
    }
}