using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(HorizontalDirectionSetter))]
    public class HorizontalDirectionSpriteChange : MonoBehaviour,IHorizontalDirectionDependant
    {
        [SerializeField] private Sprite _leftSprite;
        [SerializeField] private Sprite _rightSprite;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void OnDirectionChanged(HorizontalDirection direction)
        {
            if(direction == HorizontalDirection.None) return;
            
            _spriteRenderer.sprite = direction == HorizontalDirection.Left ? _leftSprite : _rightSprite;
        }
    }
}