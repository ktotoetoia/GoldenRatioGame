using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisual : MonoBehaviour, IModuleGraphVisual
    {
        private IModuleGraphReadOnly _source;

        public IModuleGraphReadOnly Source
        {
            get => _source;
            set
            {
                _source = value;
                RebuildSource();
            }
        }
     
        public void RebuildSource()
        {
            Debug.Log("rebuilt");
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}