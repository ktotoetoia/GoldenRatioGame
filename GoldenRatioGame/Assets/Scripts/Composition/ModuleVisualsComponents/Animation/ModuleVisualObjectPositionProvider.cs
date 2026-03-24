using IM.Abilities;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObjectPositionProvider : MonoBehaviour, IPositionProvider, IRequireReferenceModuleVisualObject
    {
        [SerializeField] private float _range;
        private IModuleVisualObject _moduleVisualObject;
        private IPositionProvider _source;
        private IFocusPointProvider _useContextAbility;
        
        private void Awake()
        {
            _useContextAbility = GetComponent<IAbilityExtension>().Ability as IFocusPointProvider;
        }

        public void SetReferenceModuleVisualObject(IModuleVisualObject moduleVisualObject)
        {
            _moduleVisualObject = moduleVisualObject;
            
            if(moduleVisualObject != null) _source = _moduleVisualObject.Transform.Transform.GetComponentInChildren<IPositionProvider>();
        }

        public Vector3 GetPosition()
        {
            if(_moduleVisualObject == null) return Vector3.zero;

            Vector3 offset = default;

            if (_useContextAbility != null)
            {
                offset = _useContextAbility.GetFocusDirection()*_range;
            }
            
            return GetSourcePosition() + offset;
        }

        private Vector3 GetSourcePosition()
        {
            return _source?.GetPosition() ?? _moduleVisualObject.Transform.Position;
        }
    }
}