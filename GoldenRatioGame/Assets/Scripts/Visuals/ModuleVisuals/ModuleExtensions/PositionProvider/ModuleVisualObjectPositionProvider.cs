using IM.Abilities;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleVisualObjectPositionProvider : MonoBehaviour, IRequireReferenceModuleVisualObject, IPositionProvider
    {
        private IModuleVisualObject _moduleVisualObject;
        private IPositionProvider _source;
        
        public void SetReferenceModuleVisualObject(IModuleVisualObject moduleVisualObject)
        {
            _moduleVisualObject = moduleVisualObject;
            _source = _moduleVisualObject.Transform.Transform.GetComponentInChildren<IPositionProvider>();
        }

        public Vector3 GetPosition()
        {
            return _source?.GetPosition() ?? _moduleVisualObject.Transform.Position;
        }
    }
}