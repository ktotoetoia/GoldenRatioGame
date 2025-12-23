using IM.Entities;

namespace IM.Modules
{
    public interface IRequireEntity
    {
        IEntity Entity { get; set; }
    }
}