using IM.Entities;

namespace IM.Modules
{
    public interface IEntityHolder
    {
        IEntity Entity { get; set; }
    }
}