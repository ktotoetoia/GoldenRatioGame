namespace IM.Graphs
{
    public class TreeModuleGraphConditions : IModuleGraphConditions
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        
        public bool CanAddModule(IModule module)
        {
            return true;
        }

        public bool CanRemoveModule(IModule module)
        {
            return true;
        }

        public bool CanConnect(IPort output, IPort input)
        {
            return !_traversal.HasPath(output.Module,input.Module) && !_traversal.HasPath(input.Module,output.Module);
        }

        public bool CanDisconnect(IConnection connection)
        {
            return true;
        }

        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanAddModule(module) && CanConnect(ownerPort, targetPort);
        }
    }
}