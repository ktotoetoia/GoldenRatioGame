using IM.Entities;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM
{
    public class ShowDocumentOnPlayerEntityDestroyed : MonoBehaviour, IRequirePlayerEntity
    {
        private UIDocument _document;
        
        private void Awake()
        {
            _document??= GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _document ??= GetComponent<UIDocument>();
            playerEntity.Destroyed += Show;
        }

        private void Show(IEntity entity)
        {    
            if(_document == null) return;
            
            _document.rootVisualElement.visible = true;
        }
    }
}