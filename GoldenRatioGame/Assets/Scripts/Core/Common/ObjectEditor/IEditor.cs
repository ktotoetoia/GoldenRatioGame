using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IEditor<out TEditor, TSnapshot>
        where TEditor : TSnapshot
    {
        TSnapshot Snapshot { get; }

        ICollection<IEditorObserver<TSnapshot>> Observers { get; }

        bool IsEditing { get; }
        bool CanSaveChanges { get; }

        TEditor BeginEdit();
        void DiscardChanges();
        bool TryApplyChanges();
    }
}