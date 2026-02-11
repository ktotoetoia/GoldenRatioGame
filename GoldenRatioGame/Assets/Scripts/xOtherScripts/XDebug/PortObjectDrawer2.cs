using UnityEngine;

namespace IM.Visuals
{
    public class PortObjectDrawer2 : MonoBehaviour
    {
        [SerializeField] private bool _draw;
        private ModuleVisualObject _visualObject;
        
        private void Awake()
        {
            _visualObject = GetComponent<ModuleVisualObject>();
        }
        
        private void OnDrawGizmos()
        {
            if(!_draw) return;
            _visualObject.PortBinder?.GizmosPreview(transform);
        }
    }
}