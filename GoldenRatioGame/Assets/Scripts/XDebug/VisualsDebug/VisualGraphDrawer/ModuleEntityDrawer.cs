using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Debug
{
    public class ModuleEntityDrawer : MonoBehaviour
    {
        private IModuleEntity _entity;
        private VisualGraphIconDrawer _visualGraphDrawer = new VisualGraphIconDrawer();
        private void Awake()
        {
            _entity = GetComponent<IModuleEntity>();
            _visualGraphDrawer.DrawSprites = false;
            _visualGraphDrawer.DrawBounds= false;
        }

        private void OnDrawGizmos()
        {
            if(_entity == null || _visualGraphDrawer == null) return;   
            
            _visualGraphDrawer.Draw(GetComponent<ModuleEntityVisuals>().GraphToDraw);
        }
    }
}