using System;
using IM.Modules;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class EditButtonsContextViewer : ContextViewer
    {
        [SerializeField] private string _saveAndExitButtonName;
        [SerializeField] private string _exitButtonName;
        private UIDocument _document;
        private Button _saveAndExitButton;
        private Button _exitButton;

        public event Action ExitButtonClicked;
        public event Action SaveAndExitButtonClicked;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _saveAndExitButton = _document.rootVisualElement.Q<Button>(_saveAndExitButtonName);
            _exitButton = _document.rootVisualElement.Q<Button>(_exitButtonName);
            _exitButton.clicked += () => ExitButtonClicked?.Invoke();
            _saveAndExitButton.clicked += () => SaveAndExitButtonClicked?.Invoke();
            
            _document.rootVisualElement.visible = false;
        }
 
        public void SetSaveAndExitButtonEnabled(bool isEnabled)
        {
            _saveAndExitButton.enabledSelf =  isEnabled;
        }
        
        public void SetExitButtonEnabled(bool isEnabled)
        {
            _exitButton.enabledSelf =  isEnabled;
        }
       
        public override void SetContext(IModuleEditingContext context)
        {
            _document.rootVisualElement.visible = true;
        }

        public override void ClearContext()
        {
            
            _document.rootVisualElement.visible = false;
        }
    }
}