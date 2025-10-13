using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ExtensibleModuleComponent : MonoBehaviour, IModuleContext
    {
        [SerializeField] private int _inputPortCount;
        [SerializeField] private int _outputPortCount;
        private IModule _module;
        
        public IModuleContextExtensions Extensions { get; private set; }

        private void Awake()
        {
            Extensions = new ModuleContextExtensions(GetComponents<IModuleExtension>());
            
            ModuleContextWrapper module =  new ModuleContextWrapper(this);

            for (int i = 0; i < _inputPortCount; i++)
                module.AddPort(new ModulePort(module,PortDirection.Input));
            for (int i = 0; i < _outputPortCount; i++)
                module.AddPort(new ModulePort(module,PortDirection.Output));
            _module = module;
        }

        public IModule GetModule()
        {
            return _module;
        }
    }
}