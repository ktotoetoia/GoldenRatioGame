namespace IM.Graphs
{
    public interface IModuleGraphSnapshotObserver
    {
        void OnGraphUpdated(IModuleGraphReadOnly graph);
    }
}