using IM.Modules;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class CurrentStatsView : ContextViewer
    {
        private UIDocument _document;
        private IModuleEditingContext _context;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
        }
        
        private void Update()
        {
            if(_context==null) return;
            
            _document.rootVisualElement.Clear();
            
        }

        public override void SetContext(IModuleEditingContext context)
        {
            _context = context;
            _document.rootVisualElement.visible = true;
        }
        
        public override void ClearContext()
        {
            _context = null;
            _document.rootVisualElement.visible = false;
        }
        
    }
}