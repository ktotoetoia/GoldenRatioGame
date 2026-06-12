using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class PopupElementPicker : MonoBehaviour
    {
        [SerializeField] private List<UIDocument> _worldViewDocuments;
        [SerializeField, Tooltip("Time in seconds to wait before showing the popup.")] private float _hoverDelay = 0.5f;
        private UIDocument _popupDocument;
        private VisualElement _currentPicked;
        private PopupElement _popupElement;
        private float _hoverTimer = 0f;
        private bool _isPopupShowing = false;

        private void Awake()
        {
            _popupDocument = GetComponent<UIDocument>();
            _popupElement = new PopupElement(preferredSide: PopupElement.PreferredSide.Backward)
            {
                CrossAxisAlignment = 1f
            };
            
            _popupDocument.rootVisualElement.Add(_popupElement);
        }

        public void UpdatePosition(Vector3 worldPosition, Vector3 screenPosition)
        {
            VisualElement newPicked = PickAt(worldPosition);

            if (_currentPicked == newPicked)
            {
                if (newPicked != null)
                {
                    if (!_isPopupShowing)
                    {
                        _hoverTimer += Time.deltaTime;

                        if (_hoverTimer >= _hoverDelay)
                        {
                            var tooltipInfoElement = new TooltipInfoElement();
                            tooltipInfoElement.Bind(newPicked as ITooltipInfo);
                            _popupElement.Show(screenPosition, tooltipInfoElement);
                            
                            _isPopupShowing = true;
                        }
                    }
                    else
                    {
                        _popupElement.SetAnchor(screenPosition);
                    }
                }
                return;
            }

            if (_isPopupShowing)
            {
                _popupElement.Hide();
                _isPopupShowing = false;
            }

            _currentPicked = newPicked;
            _hoverTimer = 0f; 
        }

        public void Clear()
        {
            _popupElement.Hide();
        }

        private VisualElement PickAt(Vector3 position)
        {
            foreach (UIDocument document in _worldViewDocuments)
            {
                if (WorldDocumentUtility.GetElementsAtPosition<ITooltipInfo>(document, position)
                        .FirstOrDefault(x => x is VisualElement) is VisualElement picked)
                    return picked;
            }

            return null;
        }
    }
}