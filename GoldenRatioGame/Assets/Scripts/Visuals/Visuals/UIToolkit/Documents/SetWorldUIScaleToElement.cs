using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class SetWorldUIScaleToElement : MonoBehaviour
    {
        [SerializeField] private UIDocument _screenDoc;
        [SerializeField] private UIDocument _worldDoc; 
        [SerializeField] private string _elementName = "MyElement";
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _scaleMultiplier = 100.0f; 

        private VisualElement _target;

        private void Start()
        {
            if (_screenDoc == null || _worldDoc == null || _uiCamera == null)
            {
                Debug.LogError("Missing assignments!");
                enabled = false;
                return;
            }

            _target = _screenDoc.rootVisualElement.Q<VisualElement>(_elementName);
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            float ppp = _target.panel.scaledPixelsPerPoint;

            Rect content = _target.contentRect;
            
            float pixelWidth = content.width * ppp;
            float pixelHeight = content.height * ppp;

            float unitsPerPixel = _uiCamera.orthographicSize * 2 / Screen.height;

            Vector2 worldSize = new Vector2(
                pixelWidth * unitsPerPixel,
                pixelHeight * unitsPerPixel
            );

            _worldDoc.worldSpaceSize = worldSize * _scaleMultiplier;
        }
    }
}