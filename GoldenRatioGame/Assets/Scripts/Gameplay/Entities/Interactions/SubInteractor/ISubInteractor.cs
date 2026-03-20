namespace IM.Entities
{
    public interface ISubInteractor
    {
        bool CanInteract(IInteractable target);
        void Interact(IInteractable target);
    }
}