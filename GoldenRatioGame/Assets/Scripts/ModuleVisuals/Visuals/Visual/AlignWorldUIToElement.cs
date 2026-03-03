using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class AlignWorldUIToElement : MonoBehaviour
    {
        [SerializeField] private UIDocument _screenDoc;
        [SerializeField] private UIDocument _worldDoc;
        [SerializeField] private string _elementName = "MyElement";
        [SerializeField] private Camera _cam;
        [SerializeField] private float _pixelsPerUnit;
        private VisualElement _target;

        private void Start()
        {
            if (_screenDoc == null || _worldDoc == null || _cam == null)
            {
                Debug.LogError("Missing assignments!");
                enabled = false;
                return;
            }

            _target = _screenDoc.rootVisualElement.Q<VisualElement>(_elementName);
            if (_target == null)
            {
                Debug.LogError($"Element '{_elementName}' not found!");
                enabled = false;
            }
        }
        
        private void LateUpdate()
        {
            if (_target == null) return;
            
            Vector2 worldPos = _cam.ScreenToWorldPoint(_target.worldBound.center * _target.panel.scaledPixelsPerPoint);
            
            _worldDoc.transform.position = worldPos;
            _worldDoc.transform.rotation = _cam.transform.rotation;
            
            Vector2 worldSize =
                _cam.ScreenToWorldPoint(_target.worldBound.max * _target.panel.scaledPixelsPerPoint)
                - _cam.ScreenToWorldPoint(_target.worldBound.min * _target.panel.scaledPixelsPerPoint);
            
            _worldDoc.worldSpaceSize = worldSize * _pixelsPerUnit;
        }
    }
}