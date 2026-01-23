using UnityEngine;

namespace IM.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererIconDrawer : MonoBehaviour, IIconDrawer
    {
        private SpriteRenderer _renderer;
        private IIcon _icon;
        private bool _isDrawing;
        
        public IIcon Icon
        {
            get
            {
                _renderer ??= GetComponent<SpriteRenderer>();
                _icon ??= new Icon(_renderer.sprite);
                
                return _icon;
            }
        }

        public bool IsDrawing
        {
            get => _isDrawing;
            set
            {
                _renderer ??= GetComponent<SpriteRenderer>();
                _renderer.enabled = value;
                _isDrawing = value;
                
            }
        }
    }
}