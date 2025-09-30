using System.Linq;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosInput : MonoBehaviour
    {
        private IModuleGraphDrawer _drawer;
        private IModuleVisualWrapper _selected;
        
        private void Awake()
        {
            _drawer = GetComponent<IModuleGraphDrawer>();
        }

        private void Update()
        {
            if(_drawer == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                _selected = _drawer.Modules.FirstOrDefault(x => x.Visual.ContainsPoint(GetMousePosition()));
            }
            
            if(_selected == null) return;

            if (Input.GetMouseButton(0))
            {
                _selected.Visual.MoveTo(GetMousePosition());
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