using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(IModuleVisualObject))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class HorizontalDirectionBasedSprite : MonoBehaviour
    {
        [SerializeField] private Sprite _leftSprite;
        [SerializeField] private Sprite _rightSprite;
        
        private SpriteRenderer _spriteRenderer;
        private IModuleVisualObject _moduleVisualObject;
        private bool _subbed;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _moduleVisualObject = GetComponent<IModuleVisualObject>();
        }

        private void Update()
        {
            if(_subbed || _moduleVisualObject.Owner == null) return;
         
            Debug.LogWarning("change this method asap");

            IEnumStateExtension<HorizontalDirection> enumExtension =_moduleVisualObject.Owner.Extensions.GetExtension<IEnumStateExtension<HorizontalDirection>>();
            
            enumExtension.ValueChanged += SetSprite;
            
            SetSprite(enumExtension.Value);
        }

        private void SetSprite(HorizontalDirection direction)
        {
            Sprite toSet = direction == HorizontalDirection.Left ? _leftSprite : _rightSprite;
            
            if(_spriteRenderer.sprite != toSet)  _spriteRenderer.sprite = toSet;
        }
    }
}