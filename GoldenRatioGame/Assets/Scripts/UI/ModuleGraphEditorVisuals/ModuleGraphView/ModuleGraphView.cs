using System;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class ModuleGraphView : MonoBehaviour
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModuleEditingContext _context;

        public ModuleGraphVisualObserver VisualObserver { get; private set; }
        public Bounds Bounds => new(transform.position, Vector3.zero);

        public void SetContext(IModuleEditingContext context)
        {
            if (VisualObserver != null) throw new InvalidOperationException();

            VisualObserver = new ModuleGraphVisualObserver(transform, false, _preset)
            {
                ShowPortsOnConnected = false,
                ShowPortsOnDisconnected = true
            };

            _context = context;
        }

        public void Update()
        {
            if (VisualObserver == null || _context == null) return;

            VisualObserver.OnSnapshotChanged(_context);
        }

        public void ClearContext()
        {
            VisualObserver.Dispose();
            VisualObserver = null;
            _context = null;
        }
    }
}