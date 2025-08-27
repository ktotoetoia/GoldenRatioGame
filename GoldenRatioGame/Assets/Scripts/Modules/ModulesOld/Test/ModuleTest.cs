using UnityEngine;

namespace IM.Modules
{
    public class ModuleTest : MonoBehaviour
    {
        private void Awake()
        {
            ModuleComposition composition = new ModuleComposition(new TestCentralModule());

            foreach (IModuleConnector centralModuleConnector in composition.CentralModule.Connectors)
            {
                composition.CentralModule.TryConnect(new ModuleBase(), centralModuleConnector);
            }
            
            GetComponent<ModuleTreeGizmosDrawer>().RootModuleBehaviour = composition.CentralModule;
        }
    }
}
