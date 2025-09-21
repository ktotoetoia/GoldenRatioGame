using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Graphs
{
    public class SafeModuleGraph : ISafeModuleGraph
    {
        private readonly ModuleGraph _inner = new();
        
        public IReadOnlyList<INode> Nodes => _inner.Nodes;
        public IReadOnlyList<IEdge> Edges => _inner.Edges;
        public IReadOnlyList<IConnection> Connections => _inner.Connections;
        public IReadOnlyList<IModule> Modules => _inner.Modules;
        public bool LogWarningsOnNullExceptions { get; set; } = true;
        public bool LogWarningInnerExceptions { get; set; } = true;

        public bool AddModule(IModule module)
        {
            if (CheckNull(module,nameof(module))|| _inner.Contains(module)) return false;
            
            return TryOrLog(() =>_inner.AddModule(module));
        }

        public bool RemoveModule(IModule module)
        {
            if (CheckNull(module,nameof(module)) || !_inner.Contains(module)) return false;
            
            return TryOrLog(() =>_inner.RemoveModule(module));
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            if (CheckNull(output, nameof(output)) || CheckNull(input, nameof(input))||output.Direction == input.Direction) return null;
            
            (output, input) = FixPorts(output, input);
            IConnection connection = null;
            
            TryOrLog(() => connection = _inner.Connect(output, input));
            
            return connection;
        }
        public void Disconnect(IConnection connection)
        {
            if(CheckNull(connection,nameof(connection)) || !_inner.Contains(connection)) return;
            
            TryOrLog(() => _inner.Disconnect(connection));
        }

        public void AddAndConnect(IModule toAdd, IModulePort addedPort, IModulePort targetPort)
        {
            AddModule(toAdd);
            Connect(addedPort, targetPort);
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

        private bool TryOrLog(Action action)
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

                return false;
            }
            
            return true;
        }
    }
}