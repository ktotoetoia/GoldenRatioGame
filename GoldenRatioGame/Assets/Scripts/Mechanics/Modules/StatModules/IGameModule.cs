using IM.Entities;

namespace IM.Modules
{
    public interface IGameModule
    {
        void AddToBuild(IEntity entity);
        void RemoveFromBuild(IEntity entity);
    }
}