using System.Linq;
using UnityEngine;
using IM.Graphs;
using IM.SelectionSystem;

namespace IM.ModuleEditor
{
    public class ModuleGraphCreator : MonoBehaviour
    {
        [SerializeField] private int _inputPorts;
        [SerializeField] private int _outputPorts;
        [SerializeField] private Bounds _initialBounds;

        private ModuleGraphEvents _graphEvents;
        private IModulePort _draggedPort;
        private IModule _draggedModule;
        private Vector2 _mouseStart;
        private Vector2 _objectStart;
        private ISelector _selector;

        private static Vector2 MouseWorld =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            _graphEvents = new ModuleGraphEvents(new ModuleGraph());
            _selector    = new GraphSelection(_graphEvents);

            if (TryGetComponent(out ModuleGraphGizmos gizmos))
                gizmos.ModuleGraph = _graphEvents;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) CreateModule();
            if (Input.GetMouseButtonDown(0)) StartDrag();
            if (Input.GetMouseButton(0))      DoDrag();
            if (Input.GetMouseButtonUp(0))    EndDrag();
            if (Input.GetMouseButtonDown(1))  HandleRightClick();
        }

        private void CreateModule()
        {
            var module = new BoundsModule
            {
                Position = _initialBounds.center,
                Size     = _initialBounds.extents
            };

            for (int i = 0; i < _inputPorts; i++)
                module.AddPort(new CirclePort(module, PortDirection.Input));

            for (int i = 0; i < _outputPorts; i++)
                module.AddPort(new CirclePort(module, PortDirection.Output));

            _graphEvents.AddModule(module);
        }

        private void StartDrag()
        {
            _mouseStart = MouseWorld;
            _selector.UpdateSelectionAt(_mouseStart);

            _draggedPort   = _selector
                .GetSelection<IModulePort>().First;
            if (_draggedPort != null) return;

            _draggedModule = _selector
                .GetSelection<IModule>().First;
            if (_draggedModule is IHavePosition pos)
                _objectStart = pos.Position;
        }

        private void DoDrag()
        {
            if (_draggedPort != null)
            {
                Debug.DrawLine(_mouseStart, MouseWorld, Color.green);
            }
            else if (_draggedModule is IHavePosition pos)
            {
                pos.Position = _objectStart + (MouseWorld - _mouseStart);
            }
        }

        private void EndDrag()
        {
            if (_draggedPort != null)
            {
                _selector.UpdateSelectionAt(MouseWorld);
                var target = _selector
                    .GetSelection<IModulePort>().First;
                if (target != null)
                    _graphEvents.Connect(_draggedPort, target);
            }

            _draggedPort   = null;
            _draggedModule = null;
        }

        private void HandleRightClick()
        {
            _selector.UpdateSelectionAt(MouseWorld);

            if (_selector.GetSelection<IModulePort>().First is IModulePort port)
            {
                _graphEvents.Disconnect(port.Connection);
                return;
            }

            if (_selector
                    .GetSelection<IModule>().First is IModule module)
                _graphEvents.RemoveModule(module);
        }
    }
}