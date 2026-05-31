using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomSpriteSelector :MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        private SpriteRenderer _spriteRenderer;
        private int _savedSpriteIndex = -1;

        public int SavedSpriteIndex
        {
            get => _savedSpriteIndex;
            set
            {
                if(_sprites.Count < _savedSpriteIndex) return;
                
                _savedSpriteIndex = value;
                
                _spriteRenderer ??= GetComponent<SpriteRenderer>();
                _spriteRenderer.sprite = _sprites[_savedSpriteIndex];
            }
        }
        
        private void Awake()
        {
            if (_savedSpriteIndex != -1) return;
            
            SavedSpriteIndex = Random.Range(0, _sprites.Count);
        }
    }
}