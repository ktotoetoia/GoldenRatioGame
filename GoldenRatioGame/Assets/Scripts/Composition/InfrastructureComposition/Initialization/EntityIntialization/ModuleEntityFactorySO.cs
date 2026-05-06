using IM.LifeCycle;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(menuName = "Factories/Module Entity Factory")]
    public class ModuleEntityFactorySO : EntityFactory
    {
        [SerializeField] private ModuleEntityEntry _entry;

        public override IEntity Create(IGameObjectFactory gameObjectFactory)
        {
            return new ModuleEntityFactory().Create(_entry, gameObjectFactory);
        }
    }
}