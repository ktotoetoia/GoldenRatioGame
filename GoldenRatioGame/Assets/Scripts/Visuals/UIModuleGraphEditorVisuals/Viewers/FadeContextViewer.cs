using System.Linq;
using IM.Modules;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class FadeContextViewer : ContextViewer
    {
        private UIDocument _document;
        private VisualElement _root;

        private const string VisibleClass = "fade-element--visible";

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement.Children().First();
        
            _root.AddToClassList("fade-element");
        }

        public override void SetContext(IModuleEditingContext context)
        {
            _root.AddToClassList(VisibleClass);
        }

        public override void ClearContext()
        {
            _root.RemoveFromClassList(VisibleClass);
        }
    }
}