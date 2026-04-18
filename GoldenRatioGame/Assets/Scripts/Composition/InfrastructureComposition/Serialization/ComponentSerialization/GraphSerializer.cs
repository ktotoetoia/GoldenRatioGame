using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public sealed class GraphSerializer
    {
         public GraphInfo Serialize(IDataModuleGraphReadOnly<IExtensibleItem> graph)
        {
            var info = new GraphInfo();

            var moduleList = graph.DataModules.Select(m => m).ToList();
            var moduleIndexMap = new Dictionary<IDataModule<IExtensibleItem>, int>();

            for (int i = 0; i < moduleList.Count; i++)
            {
                var module = moduleList[i];
                moduleIndexMap[module] = i;

                var id = module.Value.GetModuleId();
                if (string.IsNullOrEmpty(id)) continue;

                info.ModuleInfos.Add(new ModuleInfo
                {
                    Index = i,
                    ItemId = id
                });
            }

            foreach (var connection in graph.DataConnections)
            {
                var moduleA = connection.DataPort1.DataModule;
                var moduleB = connection.DataPort2.DataModule;

                if (!moduleIndexMap.TryGetValue(moduleA, out var indexA) ||
                    !moduleIndexMap.TryGetValue(moduleB, out var indexB))
                    continue;

                var portsA = moduleA.DataPorts.ToList();
                var portsB = moduleB.DataPorts.ToList();

                int portA = portsA.IndexOf(connection.DataPort1);
                int portB = portsB.IndexOf(connection.DataPort2);

                if (portA < 0 || portB < 0) continue;

                info.ConnectionInfos.Add(new ConnectionInfo
                {
                    ModuleAIndex = indexA,
                    PortAId = portA,
                    ModuleBIndex = indexB,
                    PortBId = portB
                });
            }

            return info;
        }

        public void Deserialize(
            GraphInfo info,
            IDataModuleGraph<IExtensibleItem> graph,
            IModuleEditingContext context,
            Func<string, GameObject> resolve)
        {
            if (info == null) return;

            var modules = new Dictionary<int, IDataModule<IExtensibleItem>>();

            foreach (var m in info.ModuleInfos)
            {
                var prefab = resolve(m.ItemId);
                if (prefab == null || !prefab.TryGetComponent(out IExtensibleItem item))
                    continue;

                context.AddToContext(item);

                var module = context.CreateModule(item);
                graph.Add(module);

                modules[m.Index] = module;
            }

            foreach (var c in info.ConnectionInfos)
            {
                if (!modules.TryGetValue(c.ModuleAIndex, out var moduleA) ||
                    !modules.TryGetValue(c.ModuleBIndex, out var moduleB))
                    continue;

                var portsA = moduleA.DataPorts.ToList();
                var portsB = moduleB.DataPorts.ToList();

                if ((uint)c.PortAId >= (uint)portsA.Count ||
                    (uint)c.PortBId >= (uint)portsB.Count)
                    continue;

                graph.Connect(portsA[c.PortAId], portsB[c.PortBId]);
            }
        }

        [Serializable]
        public class GraphInfo
        {
            public List<ModuleInfo> ModuleInfos = new();
            public List<ConnectionInfo> ConnectionInfos = new();
        }

        [Serializable]
        public class ModuleInfo
        {
            public int Index;
            public string ItemId;
        }

        [Serializable]
        public class ConnectionInfo
        {
            public int ModuleAIndex;
            public int PortAId;
            public int ModuleBIndex;
            public int PortBId;
        }
    }
}