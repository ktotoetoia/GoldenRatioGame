using System;

namespace IM.Entities
{
    public interface IInteractionProcess
    {
        IInteractable Target { get; }
        float EstimatedTime { get; }
        InteractionProgress Progress { get; }
        event Action Completed;
        event Action Interrupted;
    }
}