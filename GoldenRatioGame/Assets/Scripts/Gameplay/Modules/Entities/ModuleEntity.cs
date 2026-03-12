using System.Linq;
using IM.Entities;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity, IRequireInteractionProvider
    {

        public IInteractionProvider InteractionProvider { get; set; }
        public IModuleEditingContext ModuleEditingContext { get; private set; }

        private void Awake()
        {
            ModuleEditingContext = GetComponent<IModuleEditingContext>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractionProvider.GetAvailableInteractions(this).OrderBy(x => Vector3.Distance(transform.position, x.GameObject.transform.position)).FirstOrDefault()?.Interact(this);
            }
        }
    }
}