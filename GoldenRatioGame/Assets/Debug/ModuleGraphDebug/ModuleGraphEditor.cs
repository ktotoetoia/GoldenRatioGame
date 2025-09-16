using UnityEngine;
using IM.Graphs;
using IM.SelectionSystem;

namespace IM.ModuleEditor
{
    [DefaultExecutionOrder(100000)]
    public class ModuleGraphEditor : MonoBehaviour
    {
        private Vector2 _mouseStart;
        private Vector2 _objectStart;
        private ISelector _selector;
        public IModuleGraphReadOnly ModuleGraph { get; set; }

        private static Vector2 MouseWorld =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            _selector = new GraphSelection(ModuleGraph);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) StartDrag();
            if (Input.GetMouseButton(0)) DoDrag();
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
    }
}