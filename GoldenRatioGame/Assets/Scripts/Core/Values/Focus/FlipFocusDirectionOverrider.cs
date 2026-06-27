using UnityEngine;

namespace IM.Values
{
    public class FlipFocusDirectionOverrider : MonoBehaviour, IFocusDirectionOverrider
    {
        [SerializeField] private bool _flipOnRight;
        
        public void OverrideFocusDirection(Vector2 direction, float duration)
        {
            transform.localScale = direction.x switch
            {
                > 0 => new Vector3(_flipOnRight ? -1 : 1, 1, 1),
                < 0 => new Vector3(_flipOnRight ? 1 : -1, 1, 1),
                _ => transform.localScale
            };
        }
    }
}