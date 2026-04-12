using System.Collections.Generic;
using IM.Graphs;

namespace IM.LifeCycle
{
    public interface IEditor<out TEditor, TSnapshot>
        where TEditor : TSnapshot
    {
        TSnapshot Snapshot { get; }

        ICollection<IEditorObserver<TSnapshot>> Observers { get; }

        bool IsEditing { get; }
        bool CanApplyChanges { get; }

        TEditor BeginEdit();
        void DiscardChanges();
        bool TryApplyChanges();
    }
}