using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisual : MonoBehaviour, IModuleGraphVisual
    {
        private ICoreGameModule _coreModule;
        
        public IModuleGraphReadOnly Source { get; private set; }

        public void SetSource(IModuleGraphReadOnly source, ICoreGameModule coreModule)
        {
            Source = source;
            _coreModule = coreModule;
            
            RebuildSource();
        }

        private void RebuildSource()
        {
            Debug.Log("rebuilt");
        }
    }
}