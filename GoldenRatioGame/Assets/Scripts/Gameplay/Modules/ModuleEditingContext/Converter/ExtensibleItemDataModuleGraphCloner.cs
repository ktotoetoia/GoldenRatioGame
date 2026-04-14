using System.Collections.Generic;
using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class ExtensibleItemDataModuleGraphCloner : DataModuleGraphReadOnlyCloner<IExtensibleItem>
    {
        protected override IDictionary<IDataPort<TSource>, IDataPort<IExtensibleItem>> CreatePorts<TSource>(IDataModule<IExtensibleItem> targetModule, IDataModule<TSource> sourceModule)
        {            
            if (sourceModule.Value is not IExtensibleItem extensibleItem)
            {
                return base.CreatePorts(targetModule, sourceModule);
            }
            
            Dictionary<IDataPort<TSource>, IDataPort<IExtensibleItem>> dictionary = new();
            
            List<IDataPort<TSource>> sourcePorts = sourceModule.DataPorts.ToList();
            List<IDataPort<IExtensibleItem>> targetPorts = extensibleItem.PortFactory.Create(targetModule).ToList();

            for (int i = 0; i < sourcePorts.Count; i++)
            {
                dictionary.Add(sourcePorts[i], targetPorts[i]);
            }
            
            return dictionary;
        }
    }
}