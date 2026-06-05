namespace IM.Effects
{
    public interface IRestorableEffectGroupFactory : IEffectGroupFactory
    {
        public object Save(IEffectGroup modifier);
        public IEffectGroup Restore(object modifier, IEffectContext context);   
    }
}