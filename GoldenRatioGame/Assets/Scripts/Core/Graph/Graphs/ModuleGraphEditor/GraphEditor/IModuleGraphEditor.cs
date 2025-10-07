namespace IM.Graphs
{
    public interface IModuleGraphEditor
    {
        IModuleGraphReadOnly Graph { get; }
        
        bool IsEditing { get; }
        bool CanSaveChanges { get; }
        
        ICommandModuleGraph StartEditing();
        
        void CancelChanges();
        bool TrySaveChanges();
    }
}