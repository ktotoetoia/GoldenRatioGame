using IM.Modules;
using IM.UI;
using UnityEngine;

namespace IM.Visuals
{
    public class TooltipContextViewer : ContextViewer
    {
        [SerializeField] private Camera _uiCamera;
        private PopupElementPicker _popupElementPicker;
        private bool _show;

        private void Awake()
        {
            _popupElementPicker = GetComponent<PopupElementPicker>();
        }
        
        private void Update()
        {
            if(!_show) return;
            
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            _popupElementPicker.UpdatePosition(_uiCamera.ScreenToWorldPoint(Input.mousePosition),mousePosition);
        }
        
        public override void SetContext(IModuleEditingContext context) => _show = true;

        public override void ClearContext()
        {
            _show = false;
            _popupElementPicker.Clear();
        }
    }
}