using IM.LifeCycle;

namespace IM.Modules
{
    public interface IRequireEntity : IExtension
    {
        IEntity Entity { get; set; }
    }
}