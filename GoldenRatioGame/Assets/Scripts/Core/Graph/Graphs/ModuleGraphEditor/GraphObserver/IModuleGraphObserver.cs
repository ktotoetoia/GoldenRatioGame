namespace IM.Graphs
{
    public interface IModuleGraphObserver
    {
        void Update(IModuleGraphReadOnly graph);
    }
}