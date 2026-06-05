namespace IM.Effects
{
    public interface IRestorableEffectModifierFactory : IEffectModifierFactory
    {
        public object Save(IEffectModifier modifier);
        public IEffectModifier Restore(object modifier, IEffectContext context);   
    }
}