using System;
using IM.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Abilities
{
    public class ProjectileAbilityContainer : MonoBehaviour, IAbilityContainer, IProjectileEvents
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _cooldown = 3;
        [SerializeField] private float _projectileSpeed = 20;
        [SerializeField] private Sprite _iconSprite;

        public IAbilityReadOnly Ability { get; private set; }
        
        public event Action<GameObject> ProjectileGet;
        public event Action<GameObject> ProjectileRelease;
        
        private void Awake()
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease, HandleDestroy);
            
            Ability = new SendProjectileByVelocityAbility(pool, _cooldown)
            {
                Speed = _projectileSpeed,
                Icon = new Icon(_iconSprite)
            };
        }

        private GameObject Create() => Instantiate(_projectilePrefab, transform);
        private void OnGet(GameObject go)
        {
            go.SetActive(true);
            ProjectileGet?.Invoke(go);
        }
        private void OnRelease(GameObject go)
        {
            go.SetActive(false);
            ProjectileRelease?.Invoke(go);
        }
        private static void HandleDestroy(GameObject go) => Destroy(go);
    }
}