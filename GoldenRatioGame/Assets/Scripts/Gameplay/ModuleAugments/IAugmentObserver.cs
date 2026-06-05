namespace IM.Augments
{
    public interface IAugmentObserver
    {
        void OnAdded(IAugment augment);
        void OnRemoved(IAugment augment);
    }
}