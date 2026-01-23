using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class CompositeModuleGraphConditions : IModuleGraphConditions
    {
        private readonly IEnumerable<IModuleGraphConditions> _conditions;

        public CompositeModuleGraphConditions(IEnumerable<IModuleGraphConditions> conditions)
        {
            _conditions = conditions;
        }

        public bool CanAddModule(IModule module)
        {
            return _conditions.All(x => x.CanAddModule(module));
        }

        public bool CanRemoveModule(IModule module)
        {
            return _conditions.All(x => x.CanRemoveModule(module));
        }

        public bool CanConnect(IPort output, IPort input)
        {
            return _conditions.All(x => x.CanConnect(output, input));
        }

        public bool CanDisconnect(IConnection connection)
        {
            return _conditions.All(x => x.CanDisconnect(connection));
        }

        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return _conditions.All(x => x.CanAddAndConnect(module, ownerPort, targetPort));
        }
    }
}