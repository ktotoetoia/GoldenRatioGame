using IM.Graphs;

namespace IM.Modules
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