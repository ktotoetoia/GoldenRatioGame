using System;

namespace IM.Interactions
{
    public class InteractionProcess : IInteractionProcess
    {
        public IInteractable Target { get; private set; }
        public float EstimatedTime  { get; private set; }
        public float StartTime  { get; private set; }
        public InteractionProgress Progress { get; private set; } = InteractionProgress.Running;

        public event Action Completed;
        public event Action Interrupted;

        public static InteractionProcess Failed => new (null, -1,-1)
        {
            Progress = InteractionProgress.Failed
        };
        
        public InteractionProcess(IInteractable target, float estimatedTime, float startTime)
        {
            Target = target;
            EstimatedTime = estimatedTime;
            StartTime = startTime;
        }

        public void CallOnComplete()
        {
            if (Progress != InteractionProgress.Running) throw new InvalidOperationException("CallOnComplete and CallOnInterrupted can be called once total");
            
            Progress = InteractionProgress.Completed;
            Completed?.Invoke();
        }

        public void CallOnInterrupted()
        {
            if (Progress != InteractionProgress.Running) throw new InvalidOperationException("CallOnComplete and CallOnInterrupted can be called once total");

            Progress = InteractionProgress.Interrupted;
            Interrupted?.Invoke();
        }
    }
}