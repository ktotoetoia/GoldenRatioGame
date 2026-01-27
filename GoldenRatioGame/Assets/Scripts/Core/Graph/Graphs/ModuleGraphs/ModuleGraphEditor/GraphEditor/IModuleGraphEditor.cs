using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphEditor<out TModuleGraph> where TModuleGraph : IModuleGraphReadOnly
    {
        IModuleGraphReadOnly Graph { get; }
        ICollection<IModuleGraphSnapshotObserver> Observers { get; }
        bool IsEditing { get; }
        bool CanSaveChanges { get; }
        
        TModuleGraph StartEditing();
        void CancelChanges();
        bool TrySaveChanges();
    }
}