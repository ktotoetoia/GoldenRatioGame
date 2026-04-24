using System;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class ModuleGraphView : ContextViewer
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private IModuleEditingContext _context;

        public ModuleGraphVisualObserver VisualObserver { get; private set; }

        private void Update()
        {
            if (VisualObserver == null || _context == null) return;

            VisualObserver.OnSnapshotChanged(_context);
        }

        public override void SetContext(IModuleEditingContext context)
        {
            if (VisualObserver != null) throw new InvalidOperationException();

            VisualObserver = new ModuleGraphVisualObserver(transform, false, _preset)
            {
                ShowPortsOnConnected = false,
                ShowPortsOnDisconnected = true
            };

            _context = context;
        }

        public override void ClearContext()
        {
            VisualObserver.Dispose();
            VisualObserver = null;
            _context = null;
        }
    }
}