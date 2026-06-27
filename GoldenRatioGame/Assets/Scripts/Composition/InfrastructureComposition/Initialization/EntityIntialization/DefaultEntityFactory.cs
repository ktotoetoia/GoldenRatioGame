using IM.Factions;
using IM.LifeCycle;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(menuName = "Factories/Default Entity Factory")]
    public class DefaultEntityFactory : EntityFactory
    {
        [SerializeField] private EntityEntry _entry;
        
        public override IEntity Create(IGameObjectFactory gameObjectFactory)
        {
            GameObject created = gameObjectFactory.Create(_entry.EntityGameObject,false);

            created.transform.position = new Vector3(Random.Range(_entry.Bounds.min.x, _entry.Bounds.max.x), Random.Range(_entry.Bounds.min.y, _entry.Bounds.max.y));
            
            if (created.TryGetComponent(out IFactionMember factionMember)) factionMember.Faction = _entry.Faction;
            
            return created.GetComponent<IEntity>();
        }
    }
}