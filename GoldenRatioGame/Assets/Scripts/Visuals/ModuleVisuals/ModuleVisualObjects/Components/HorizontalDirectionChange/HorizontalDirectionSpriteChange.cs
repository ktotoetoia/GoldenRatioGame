using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class HorizontalDirectionSpriteChange : MonoBehaviour,IHorizontalDirectionDependant
    {
        [SerializeField] private Sprite _leftSprite;
        [SerializeField] private Sprite _rightSprite;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnDirectionChanged(HorizontalDirection direction)
        {
            if(direction == HorizontalDirection.None) return;
            
            _spriteRenderer.sprite = direction == HorizontalDirection.Left ? _leftSprite : _rightSprite;
        }
    }
}