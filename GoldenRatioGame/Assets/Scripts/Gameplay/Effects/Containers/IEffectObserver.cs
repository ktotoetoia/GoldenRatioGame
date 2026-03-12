namespace IM.Effects
{
    public interface IEffectObserver
    {
        void OnEffectGroupAdded(IEffectGroup group);
        void OnEffectGroupRemoved(IEffectGroup group);
    }
}