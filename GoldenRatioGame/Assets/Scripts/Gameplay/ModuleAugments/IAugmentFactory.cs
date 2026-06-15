using IM.LifeCycle;

namespace IM.Augments
{
    public interface IAugmentFactory : IRestorableFactory<IAugment,IAugmentContext>
    {
    }
}