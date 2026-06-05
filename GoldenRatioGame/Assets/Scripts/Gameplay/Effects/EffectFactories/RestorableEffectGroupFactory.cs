namespace IM.Effects
{
    public abstract class RestorableEffectGroupFactory : EffectGroupFactory, IRestorableEffectGroupFactory
    {
        public abstract object Save(IEffectGroup modifier);
        public abstract IEffectGroup Restore(object modifier, IEffectContext ctx);
    }
}