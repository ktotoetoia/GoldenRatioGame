using UnityEngine;

namespace IM.Modules
{
    public class ModuleEditingContextItemsParentSetter : MonoBehaviour
    {
        [SerializeField] private GameObject _moduleEditingContextEventsSource;
        [SerializeField] private Transform _defaultModulesTransformParent;
        [SerializeField] private Transform _parentOfOnRemoved;
        private IModuleEditingContextEvents _moduleEditingContext;

        private void Awake()
        {
            _moduleEditingContext = _moduleEditingContextEventsSource.GetComponent<IModuleEditingContextEvents>();
            
            _moduleEditingContext.AddedToContext += OnAdded;
            _moduleEditingContext.RemovedFromContext += OnRemoved;
        }

        private void OnAdded(IExtensibleModule module)
        {
            if (module is not MonoBehaviour moduleMono) return;
            
            moduleMono.transform.SetParent(_defaultModulesTransformParent,false);
            moduleMono.transform.position = _defaultModulesTransformParent.position;
        }

        private void OnRemoved(IExtensibleModule module)
        {
            if (module is not MonoBehaviour moduleMono) return;
            
            moduleMono.transform.position = transform.position;
            moduleMono.transform.SetParent(_parentOfOnRemoved.parent);
        }
    }
}