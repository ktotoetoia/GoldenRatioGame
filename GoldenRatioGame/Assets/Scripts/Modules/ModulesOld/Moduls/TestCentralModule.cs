namespace IM.Modules
{
    public class TestCentralModule : ModuleBase
    {
        public TestCentralModule()
        {
            _connectors.Add(new ModuleConnector(this));
            _connectors.Add(new ModuleConnector(this));
            _connectors.Add(new ModuleConnector(this));
            _connectors.Add(new ModuleConnector(this));
        }
    }
}