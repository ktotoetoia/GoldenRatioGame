namespace IM.Entities
{
    public interface IRequireInteractionProvider
    {
        IInteractionProvider InteractionProvider { get; set; }
    }
}