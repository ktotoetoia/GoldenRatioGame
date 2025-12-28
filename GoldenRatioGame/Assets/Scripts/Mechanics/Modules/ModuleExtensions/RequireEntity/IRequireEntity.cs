using IM.Entities;

namespace IM.Modules
{
    public interface IRequireEntity : IModuleExtension
    {
        IEntity Entity { get; set; }
    }
}