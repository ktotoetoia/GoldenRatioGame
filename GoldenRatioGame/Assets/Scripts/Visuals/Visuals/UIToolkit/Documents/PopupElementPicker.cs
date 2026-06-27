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

        private PopupElement _popup;
        private TooltipInfoElement _tooltip;
        private StatPreviewContainer _statPreviews;
        
        private VisualElement _pickedElement;
        private float _hoverTimer;
        private bool _isShowing;

        private void Awake()
        {
            UIDocument popupDocument = GetComponent<UIDocument>();

            _popup = new PopupElement(preferredSide: PopupElement.PreferredSide.Backward)
            {
                CrossAxisAlignment = 1f
            };

            _tooltip = new TooltipInfoElement();
            _statPreviews = new StatPreviewContainer(GetComponents<IStatPreviewer>());

            popupDocument.rootVisualElement.Add(_popup);
        }

        public void UpdatePosition(
            Vector3 worldPosition,
            Vector3 screenPosition)
        {
            VisualElement pickedElement = PickAt(worldPosition);
            Vector2 panelPosition = GetPanelPosition(screenPosition);

            if (pickedElement != _pickedElement)
            {
                SetPickedElement(pickedElement);
                return;
            }

            if (_pickedElement == null) return;

            if (!_isShowing)
            {
                _hoverTimer += Time.deltaTime;

                if (_hoverTimer >= _hoverDelay)
                    Show(panelPosition);

                return;
            }

            _statPreviews.UpdatePreviews(_pickedElement);
            _popup.SetAnchor(panelPosition);
        }

        public void Clear()
        {
            SetPickedElement(null);
        }

        private void Show(Vector2 panelPosition)
        {
            if (_pickedElement is not ITooltipInfo tooltipInfo)
                return;

            _tooltip.Bind(tooltipInfo);
            _statPreviews.Bind(tooltipInfo.Item);

            _tooltip.SetAdditionalInfo(_statPreviews.HasContent ? _statPreviews : null);

            _popup.Show(panelPosition, _tooltip);
            _isShowing = true;
        }

        private void SetPickedElement(VisualElement element)
        {
            Hide();

            _pickedElement = element;
            _hoverTimer = 0f;
        }

        private void Hide()
        {
            if (_isShowing) _popup.Hide();

            _statPreviews.Unbind();
            _tooltip.Unbind();

            _isShowing = false;
        }

        private Vector2 GetPanelPosition(Vector3 screenPosition)
        {
            return RuntimePanelUtils.ScreenToPanel(_popup.panel, screenPosition);
        }

        private VisualElement PickAt(Vector3 position)
        {
            foreach (UIDocument document in _worldViewDocuments)
            {
                VisualElement picked = WorldDocumentUtility.GetElementsAtPosition<ITooltipInfo>(document, position).OfType<VisualElement>().FirstOrDefault();

                if (picked != null) return picked;
            }

            return null;
        }
    }
}