using IM.Base;
using UnityEngine;

namespace IM.Abilities
{
    public class ProjectileFactory : IFactory<GameObject>
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        
        public ProjectileFactory(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Create()
        {
            return Object.Instantiate(_prefab, _parent.position, _parent.rotation);
        }
    }
}