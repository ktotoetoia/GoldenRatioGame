using IM.Entities;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class ModuleEntity : DefaultEntity, IModuleEntity
    {
        public IModuleEditingContext ModuleEditingContext { get; private set; }

        private void Awake()
        {
            ModuleEditingContext = GetComponent<IModuleEditingContext>();
        }
    }
}