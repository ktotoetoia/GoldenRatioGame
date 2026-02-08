using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class PortObjectDrawer : MonoBehaviour
    {
        [SerializeField] private PortBinderBase _portBinder;

        private void OnDrawGizmos()
        {
            _portBinder?.GizmosPreview(transform);
        }
    }
}