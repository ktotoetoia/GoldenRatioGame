namespace IM.Graphs
{
    public interface ISingleDraftContainer<out TDraft, out TRead> where TDraft : TRead
    {
        TRead Source { get; }
        TRead Draft { get; }
        
        TDraft GetEditableDraft();
        void CommitDraft();
    }
}