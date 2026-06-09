using System.Collections.Generic;
using IM.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class StatPreviewContainer : MonoBehaviour, IStatPreviewContainer
    {
        [SerializeField] private string _containerName = "StatsContainer";
        private readonly List<IStatPreviewer>  _statPreviewers = new();
        private readonly Dictionary<IStatPreviewer, VisualElement> _statPreviewerMap = new();
        private UIDocument _document;
        private VisualElement _visualElement;
        private IModuleEntity _entity;
        private IModuleEditingContextReadOnly _contextReadOnly;
        
        private void Awake()
        {
            _document  = GetComponent<UIDocument>();
            _visualElement = _document.rootVisualElement.Q<VisualElement>(_containerName);
            _document.rootVisualElement.visible = false;
            GetComponents(_statPreviewers);
        }

        private void Update()
        {
            foreach ((IStatPreviewer statPreviewer,VisualElement element) in _statPreviewerMap) 
                statPreviewer.UpdatePreview(element,_entity, _contextReadOnly);
        }

        public void StartPreview(IModuleEntity entity, IModuleEditingContextReadOnly currentContext)
        {
            _entity = entity;
            _contextReadOnly = currentContext;
            _document.rootVisualElement.visible = true;
            
            foreach (var statPreviewer in _statPreviewers)
            {
                VisualElement preview = statPreviewer.GetPreview(entity, currentContext);
                
                _visualElement.Add(preview);
                _statPreviewerMap[statPreviewer] = preview;
            }
        }

        public void StopPreview()
        {
            _entity = null;
            _contextReadOnly = null;
            _document.rootVisualElement.visible = false;
            _statPreviewerMap.Clear();
            _visualElement.Clear();
        }
    }
}