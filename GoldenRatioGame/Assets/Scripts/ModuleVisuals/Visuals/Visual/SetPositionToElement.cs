using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class SetPositionToElement : MonoBehaviour
    {
        [SerializeField] private Transform _transformToSet;
        [SerializeField] private UIDocument _elementSource;
        [SerializeField] private string _elementName = "MyElement";
        [SerializeField] private Camera _uiCamera;
        private VisualElement _target;

        private void Start()
        {
            if (_elementSource == null || _uiCamera == null)
            {
                Debug.LogError("Missing assignments!");
                enabled = false;
                return;
            }
            
            _target = _elementSource.rootVisualElement.Q<VisualElement>(_elementName);
            
            if (_target == null)
            {
                Debug.LogError($"Element '{_elementName}' not found!");
                enabled = false;
            }
        }
        
        private void LateUpdate()
        {
            if (_target == null) return;
            
            Vector2 worldPos = _uiCamera.ScreenToWorldPoint(_target.worldBound.center * _target.panel.scaledPixelsPerPoint);
            
            _transformToSet.position = worldPos;
            _transformToSet.rotation = _uiCamera.transform.rotation;
        }
    }
}