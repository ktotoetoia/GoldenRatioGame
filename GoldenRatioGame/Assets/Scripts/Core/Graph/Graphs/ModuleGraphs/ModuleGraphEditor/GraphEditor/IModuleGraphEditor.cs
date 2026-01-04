using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphEditor<out TModuleGraph> where TModuleGraph : IModuleGraphReadOnly
    {
        IModuleGraphReadOnly Graph { get; }
        
        bool IsEditing { get; }
        bool CanSaveChanges { get; }
        
        TModuleGraph StartEditing();
        void CancelChanges();
        bool TrySaveChanges();
        
        
        IEnumerable<IModuleGraphSnapshotObserver> Observers { get; }
        void AddObserver(IModuleGraphSnapshotObserver observer);
        void RemoveObserver(IModuleGraphSnapshotObserver observer);
    }
}