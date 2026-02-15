using UnityEngine;

namespace IM.Visuals
{
    public class PortObjectDrawer2 : MonoBehaviour
    {
        [SerializeField] private bool _draw;
        
        private void OnDrawGizmos()
        {
            if(!_draw) return;
            GetComponent<ModuleVisualObject>()?.PortBinder?.GizmosPreview(transform);
        }
    }
}