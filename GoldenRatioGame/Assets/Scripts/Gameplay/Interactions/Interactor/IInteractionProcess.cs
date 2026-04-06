using System;

namespace IM.Interactions
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