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

            Vector2 centerPoints = _target.worldBound.center;
            float ppp = _target.panel.scaledPixelsPerPoint;
            
            Vector3 screenPos = new Vector3(
                centerPoints.x * ppp,
                Screen.height - (centerPoints.y * ppp), 
                _uiCamera.nearClipPlane + 1.0f
            );

            Vector3 worldPos = _uiCamera.ScreenToWorldPoint(screenPos);

            worldPos.z = 0;
            _transformToSet.position = worldPos;
            _transformToSet.rotation = _uiCamera.transform.rotation;
        }
    }
}