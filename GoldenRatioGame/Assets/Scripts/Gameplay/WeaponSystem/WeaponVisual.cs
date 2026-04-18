using IM.Transforms;
using UnityEngine;

namespace IM.WeaponSystem
{
    public class WeaponVisual : MonoBehaviour, IWeaponVisual
    {
        [SerializeField] private Renderer _renderer;
        
        public ITransform Transform { get; private set; }
        public int Order { get => _renderer.sortingOrder; set => _renderer.sortingOrder = value; }
        public bool Visible { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
        public int Layer { get => gameObject.layer; set => gameObject.layer = value; }

        private void Awake()
        {
            Transform = GetComponent<ITransform>();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}