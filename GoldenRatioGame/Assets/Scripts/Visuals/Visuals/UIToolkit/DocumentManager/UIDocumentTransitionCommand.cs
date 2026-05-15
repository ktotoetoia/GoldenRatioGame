using IM.Commands;

namespace IM.UI
{
    public class UIDocumentTransitionCommand : Command
    {
        private readonly DocumentTransitionInfo _info;
        
        public UIDocumentTransitionCommand(DocumentTransitionInfo info)
        {
            _info = info;
        }

        protected override void InternalExecute()
        {
            if (_info.IsOverlay) 
                _info.ToDocument.sortingOrder = _info.FromDocument.sortingOrder + 1;
            else
                _info.FromDocument.rootVisualElement.visible = false;

            _info.ToDocument.rootVisualElement.visible = true;
        }

        protected override void InternalUndo()
        {
            _info.ToDocument.rootVisualElement.visible = false;
            
            if (_info.IsOverlay) 
                _info.ToDocument.sortingOrder = _info.DefaultSortingOrder;
            else
                _info.FromDocument.rootVisualElement.visible = true;
        }
    }
}