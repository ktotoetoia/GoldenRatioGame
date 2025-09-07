using IM.Entities;

namespace IM.Modules
{
    public interface IEntityModule
    {
        void AddToBuild(IEntity entity);
        void RemoveFromBuild(IEntity entity);
    }
}