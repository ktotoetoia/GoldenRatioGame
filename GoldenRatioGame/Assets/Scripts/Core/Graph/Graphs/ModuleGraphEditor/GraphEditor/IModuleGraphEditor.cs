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
        
        
        IEnumerable<IModuleGraphObserver> Observers { get; }
        void AddObserver(IModuleGraphObserver observer);
        void RemoveObserver(IModuleGraphObserver observer);
    }
}