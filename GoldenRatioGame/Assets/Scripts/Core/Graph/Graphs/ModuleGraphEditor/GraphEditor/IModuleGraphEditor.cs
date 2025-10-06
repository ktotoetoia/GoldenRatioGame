namespace IM.Graphs
{
    public interface IModuleGraphEditor
    {
        IModuleGraphReadOnly Graph { get; }
        
        bool IsEditing { get; }
        bool CanSaveChanges { get; }
        
        IModuleGraph StartEditing();
        
        void CancelChanges();
        bool TrySaveChanges();
    }
}