namespace IM.Graphs
{
    public interface IEditorObserver<in TSnapshot>
    {
        void OnSnapshotChanged(TSnapshot snapshot);
    }
}