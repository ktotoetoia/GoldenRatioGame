using System.Linq;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosInput : MonoBehaviour
    {
        private IModuleGraphDrawer _drawer;
        private ModuleVisual _selected;
        
        private void Awake()
        {
            _drawer = GetComponent<IModuleGraphDrawer>();
        }

        private void Update()
        {
            if(_drawer == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                _selected = _drawer.Modules.FirstOrDefault(x => x.ContainsPoint(GetMousePosition()));
            }
            
            if(_selected == null) return;

            if (Input.GetMouseButton(0))
            {
                _selected.MoveTo(GetMousePosition());
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selected = null;
            }
        }

        private Vector2 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}