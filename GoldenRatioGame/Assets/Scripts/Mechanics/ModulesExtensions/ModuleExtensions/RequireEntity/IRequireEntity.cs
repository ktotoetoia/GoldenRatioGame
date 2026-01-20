using IM.Entities;

namespace IM.Modules
{
    public interface IRequireEntity : IExtension
    {
        IEntity Entity { get; set; }
    }
}