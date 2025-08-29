using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Graphs
{
    public class SafeModuleGraph : IModuleGraph
    {
        private readonly ModuleGraph _inner = new();
        
        public IReadOnlyList<INode> Nodes => _inner.Nodes;
        public IReadOnlyList<IEdge> Edges => _inner.Edges;
        public IEnumerable<IModuleConnection> Connections => _inner.Connections;
        public IEnumerable<IModule> Modules => _inner.Modules;
        public bool LogWarningsOnNullExceptions { get; set; } = true;
        public bool LogWarningInnerExceptions { get; set; } = true;

        public void AddModule(IModule module)
        {
            if (CheckNull(module,nameof(module))|| _inner.Contains(module)) return;
            
            TryOrLog(() =>_inner.AddModule(module));
        }

        public void RemoveModule(IModule module)
        {
            if (CheckNull(module,nameof(module)) || !_inner.Contains(module)) return;
            
            TryOrLog(() =>_inner.RemoveModule(module));
        }

        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            if (CheckNull(output, nameof(output)) || CheckNull(input, nameof(input))||output.Direction == input.Direction) return null;
            
            (output, input) = FixPorts(output, input);
            IModuleConnection moduleConnection = null;
            
            TryOrLog(() => moduleConnection = _inner.Connect(output, input));
            
            return moduleConnection;
        }
        public void Disconnect(IModuleConnection connection)
        {
            if(CheckNull(connection,nameof(connection)) || !_inner.Contains(connection)) return;
            
            TryOrLog(() => _inner.Disconnect(connection));
        }

        public void Clear()
        {
            _inner.Clear();
        }
        
        private bool CheckNull(object obj, string name)
        {
            if (obj is not null)
            {
                return false;
            }
            
            if (LogWarningsOnNullExceptions)
            {
                Debug.LogWarning($"{name} is null");
            }

            return true;
        }

        private (IModulePort, IModulePort) FixPorts(IModulePort output, IModulePort input)
        {
            return output.Direction == PortDirection.Input ? (input, output) : (output, input);
        }

        private void TryOrLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (LogWarningInnerExceptions)
                {
                    Debug.LogWarning(e);
                }
            }
        }
    }
}