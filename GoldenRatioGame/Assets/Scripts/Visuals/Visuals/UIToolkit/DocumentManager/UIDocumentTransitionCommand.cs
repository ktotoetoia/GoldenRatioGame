using IM.Commands;
using UnityEngine.UIElements;

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
                SetVisible(_info.FromDocument, false);

            SetVisible(_info.ToDocument, true);
        }

        protected override void InternalUndo()
        {
            SetVisible(_info.ToDocument, false);
            
            if (_info.IsOverlay) 
                _info.ToDocument.sortingOrder = _info.DefaultSortingOrder;
            else
                SetVisible(_info.FromDocument, true);
        }

        private void SetVisible(UIDocument uIDocument, bool isVisible)
        {
            uIDocument.rootVisualElement.visible = isVisible;
        }
    }
}