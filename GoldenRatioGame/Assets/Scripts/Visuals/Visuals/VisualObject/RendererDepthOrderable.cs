using UnityEngine;

namespace IM.Visuals
{
    public class RendererDepthOrderable : MonoBehaviour, IDepthOrderable
    {
        private Renderer _renderer;

        public Vector3 ReferencePoint => _renderer.bounds.min;

        public int Order
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }
        
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }
    }
}