using System.Collections.Generic;
using IM.Abilities;
using IM.Entities;
using IM.Factions;
using IM.Items;
using IM.Values;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Modules
{
    public class FireProjectileAbilityExtension : MonoBehaviour, IAbilityExtension, IRequireEntity
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _cooldown = 3;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private Sprite _iconSprite;
        private IEntity _entity;
        private IFactionMemberReadOnly _factionMember;
        
        private readonly HashSet<GameObject> _instantiatedProjectiles = new();
        
        public IAbilityReadOnly Ability { get; private set; }
        
        public IEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;

                if (_entity?.GameObject.TryGetComponent(out _factionMember) ?? false)
                {
                    UpdateProjectilesFaction();
                }
            }
        }

        private void Awake()
        {
            IObjectPool<GameObject> projectilePool = new ObjectPool<GameObject>(
                Create, OnGet, OnRelease, HandleDestroy);

            Ability = new SendProjectileByVelocityAbility(projectilePool, GetComponent<IPositionProvider>(), _cooldown)
            {
                Speed = _projectileSpeed,
                Icon = new Icon(_iconSprite)
            };
        }

        private void UpdateProjectilesFaction()
        {
            if (_factionMember == null) return;

            foreach (var projectile in _instantiatedProjectiles)
            {
                if (projectile != null && projectile.TryGetComponent(out IRequireFactionMember req))
                {
                    req.FactionMemberReadOnly = _factionMember;
                }
            }
        }

        private GameObject Create()
        {
            GameObject created = Instantiate(_projectilePrefab, transform);
            _instantiatedProjectiles.Add(created);

            if (created.TryGetComponent(out IRequireFactionMember requireFactionMember))
            {
                requireFactionMember.FactionMemberReadOnly = _factionMember;
            }
            
            return created;
        }

        private void OnGet(GameObject go) => go.SetActive(true);

        private void OnRelease(GameObject go) => go.SetActive(false);

        private void HandleDestroy(GameObject go)
        {
            _instantiatedProjectiles.Remove(go);
            Destroy(go);
        }

        private void OnDestroy()
        {
            _instantiatedProjectiles.Clear();
        }
    }
}