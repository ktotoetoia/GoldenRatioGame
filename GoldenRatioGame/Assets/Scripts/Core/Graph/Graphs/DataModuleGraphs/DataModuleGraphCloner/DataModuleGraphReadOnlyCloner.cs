using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class DataModuleGraphReadOnlyCloner<TTarget> : IDataModuleGraphCloner<IDataModuleGraphReadOnly<TTarget>, TTarget>
    {
        public virtual IDataModuleGraphReadOnly<TTarget> Copy<TSource>(
            IDataModuleGraphReadOnly<TSource> sourceGraph, 
            Func<TSource, TTarget> mapper)
        {
            List<IDataModule<TTarget>> modules = new List<IDataModule<TTarget>>();
            List<IDataConnection<TTarget>> connections = new List<IDataConnection<TTarget>>();

            Dictionary<IDataPort<TSource>, IDataPort<TTarget>> portsMap = MapModulesAndPorts(sourceGraph, mapper, modules.Add);
            
            foreach (IDataConnection<TSource> sourceConn in sourceGraph.DataConnections)
            {
                if (TryMapConnection(sourceConn, portsMap, out IDataConnection<TTarget> targetConn))
                {
                    connections.Add(targetConn);
                }
            }

            return CreateGraph(modules, connections);
        }

        public void Apply<TSource>(
            IDataModuleGraphReadOnly<TSource> sourceGraph, 
            IDataModuleGraph<TTarget> targetGraph,
            Func<TSource, TTarget> mapper)
        {
            Dictionary<IDataPort<TSource>, IDataPort<TTarget>> portsMap = MapModulesAndPorts(sourceGraph, mapper, targetGraph.Add);

            foreach (IDataConnection<TSource> sourceConn in sourceGraph.DataConnections)
            {
                targetGraph.Connect(portsMap[sourceConn.DataPort1], portsMap[sourceConn.DataPort2]);
            }
        }

        private Dictionary<IDataPort<TSource>, IDataPort<TTarget>> MapModulesAndPorts<TSource>(
            IDataModuleGraphReadOnly<TSource> source, 
            Func<TSource, TTarget> mapper,
            Action<IDataModule<TTarget>> onModuleCreated)
        {
            Dictionary<IDataPort<TSource>, IDataPort<TTarget>> portsMap = new Dictionary<IDataPort<TSource>, IDataPort<TTarget>>();

            foreach (IDataModule<TSource> sourceModule in source.DataModules)
            {
                DataModule<TTarget> targetModule = CreateModule(mapper(sourceModule.Value));
                
                foreach (IDataPort<TSource> sourcePort in sourceModule.DataPorts)
                {
                    IDataPort<TTarget> targetPort = CreatePort(targetModule);
                    targetModule.AddPort(targetPort);
                    portsMap[sourcePort] = targetPort;
                }
                
                onModuleCreated(targetModule);
            }

            return portsMap;
        }

        private bool TryMapConnection<TSource>(
            IDataConnection<TSource> sourceConn, 
            Dictionary<IDataPort<TSource>, IDataPort<TTarget>> portsMap,
            out IDataConnection<TTarget> targetConnection)
        {
            if (portsMap.TryGetValue(sourceConn.DataPort1, out IDataPort<TTarget> p1) && 
                portsMap.TryGetValue(sourceConn.DataPort2, out IDataPort<TTarget> p2))
            {
                targetConnection = CreateConnection(p1, p2);
                return true;
            }

            targetConnection = null;
            return false;
        }

        protected virtual DataModule<TTarget> CreateModule(TTarget value) 
            => new(value);

        protected virtual IDataPort<TTarget> CreatePort(IDataModule<TTarget> parent) 
            => new DataPort<TTarget>(parent);

        protected virtual IDataConnection<TTarget> CreateConnection(IDataPort<TTarget> p1, IDataPort<TTarget> p2)
        {
            DataConnection<TTarget> conn = new DataConnection<TTarget>(p1, p2);
            p1.Connect(conn);
            p2.Connect(conn);
            return conn;
        }

        protected virtual IDataModuleGraphReadOnly<TTarget> CreateGraph(
            IEnumerable<IDataModule<TTarget>> modules, 
            IEnumerable<IDataConnection<TTarget>> connections)
            => new DataModuleGraphReadOnly<TTarget>(modules.ToList(), connections.ToList());
    }
}