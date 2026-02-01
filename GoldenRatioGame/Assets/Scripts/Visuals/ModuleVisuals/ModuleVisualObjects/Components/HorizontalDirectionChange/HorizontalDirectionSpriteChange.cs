using System;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(HorizontalDirectionSetter))]
    [RequireComponent(typeof(SpriteRenderer))]
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
            _spriteRenderer.sprite = direction == HorizontalDirection.Left ? _leftSprite : _rightSprite;
        }
    }
}