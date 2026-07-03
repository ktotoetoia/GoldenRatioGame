using IM.Modules;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class DocumentContextViewer : ContextViewer
    {
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            if(_document) _document.rootVisualElement.visible = false;
        }
        
        public override void SetContext(IModuleEditingContext context)
        {
            if(!_document) return;
            
            _document.rootVisualElement.visible = true;
        }

        public override void ClearContext()
        {
            if(!_document) return;

            _document.rootVisualElement.visible = false;
        }
    }
}