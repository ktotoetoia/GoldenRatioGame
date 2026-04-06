using System;
using IM.Interactions;
using IM.Modules;
using UnityEngine;

namespace IM.Entities
{
    public class ExtensibleModuleInteractor : MonoBehaviour, ISubInteractor
    {
        [SerializeField] private GameObject _moduleEditingContextSource;
        private IModuleEditingContext _moduleEditingContext;

        private void Awake()
        {
            if (!_moduleEditingContextSource) throw new MissingComponentException($"{nameof(_moduleEditingContextSource)} was not initialized");
            
            _moduleEditingContext = _moduleEditingContextSource.GetComponent<IModuleEditingContext>();
        }
        
        public bool CanInteract(IInteractable target)
        {
            return CanInteract(target, out IExtensibleModule module);
        }

        public void Interact(IInteractable target)
        {
            if (!CanInteract(target, out IExtensibleModule module))
                throw new ArgumentException($"Cannot interact with this argument: {target.GameObject}");
            
            _moduleEditingContext.AddToContext(module);
        }

        private bool CanInteract(IInteractable target, out IExtensibleModule module)
        {
            return target.GameObject.TryGetComponent(out  module) && module.ModuleState == ModuleState.Show;
        }
    }
}