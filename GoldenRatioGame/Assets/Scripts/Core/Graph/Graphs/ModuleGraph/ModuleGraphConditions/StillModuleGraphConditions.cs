namespace IM.Graphs
{
    public class StillModuleGraphConditions : IModuleGraphConditions
    {
        private readonly bool _condition;

        public StillModuleGraphConditions(bool condition = true)
        {
            _condition = condition;
        }
        
        public bool CanAddModule(IModule module)
        {
            return _condition;
        }

        public bool CanRemoveModule(IModule module)
        {            
            return _condition;
        }

        public bool CanConnect(IModulePort output, IModulePort input)
        {
            return _condition;
        }

        public bool CanDisconnect(IConnection connection)
        {
            return _condition;
        }

        public bool CanAddAndConnect(IModule module, IModulePort ownerPort, IModulePort targetPort)
        {
            return _condition;
        }
    }
}