namespace IM.Graphs
{
    public interface IModuleGraphObserver
    {
        void OnGraphUpdated(IModuleGraphReadOnly graph);
    }
}