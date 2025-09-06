using UnityEngine;
using IM.Graphs;
using IM.SelectionSystem;

namespace IM.ModuleEditor
{
    public class ModuleGraphEditor : MonoBehaviour
    {
        [SerializeField] private int _inputPorts;
        [SerializeField] private int _outputPorts;
        [SerializeField] private Bounds _initialBounds;
        private ModuleGraphEvents _graphEvents;
        private Vector2 _mouseStart;
        private Vector2 _objectStart;
        private ISelector _selector;

        private static Vector2 MouseWorld =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            _graphEvents = new ModuleGraphEvents(new ModuleGraph());
            _selector = new GraphSelection(_graphEvents);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) CreateModule();
            if (Input.GetMouseButtonDown(0)) StartDrag();
            if (Input.GetMouseButton(0)) DoDrag();
            if (Input.GetMouseButtonUp(0)) EndDrag();
            if (Input.GetMouseButtonDown(1)) HandleRightClick();
        }

        private void CreateModule()
        {
            var module = new BoundsModule
            {
                Position = _initialBounds.center,
                Size = _initialBounds.extents
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
            _selector.UpdateSelectionAt(MouseWorld);

            if (_selector.GetSelection<IModulePort>().First != null) return;

            if (_selector.GetSelection<IModule>().First is IHavePosition pos)
                _objectStart = pos.Position;
        }

        private void DoDrag()
        {
            if (_selector.GetSelection<IModulePort>().First != null)
            {
                Debug.DrawLine(_mouseStart, MouseWorld, Color.green);
            }
            if (_selector.GetSelection<IModule>().First is IHavePosition pos)
            {
                pos.Position = _objectStart + (MouseWorld - _mouseStart);
            }
        }

        private void EndDrag()
        {
            if (_selector.GetSelection<IModulePort>().First is { } port1 && _selector.SelectionProvider.SelectAt<IModulePort>(MouseWorld).First is { } port2)
            {
                _graphEvents.Connect(port1, port2);
            }
        }

        private void HandleRightClick()
        {
            _selector.UpdateSelectionAt(MouseWorld);

            if (_selector.GetSelection<IModulePort>().First is { } port)
            {
                _graphEvents.Disconnect(port.Connection);
                
                return;
            }

            if (_selector.GetSelection<IModule>().First is { } module)
            {
                _graphEvents.RemoveModule(module);
            }
        }
    }
}