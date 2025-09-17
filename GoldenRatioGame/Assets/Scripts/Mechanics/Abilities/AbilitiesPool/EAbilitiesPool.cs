using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Abilities
{
    public class EAbilitiesPool : MonoBehaviour, IAbilitiesPool
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileCooldown;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _blinkDistance;
        [SerializeField] private float _blinkCooldown;
        private readonly List<IAbility> _activeAbilities = new();
        public IEnumerable<IAbility> Abilities => _activeAbilities;

        private void Awake()
        {
            _activeAbilities.Add(new BlinkForwardAbility(GetDirection,GetComponent<Rigidbody2D>(),_blinkCooldown));
            _activeAbilities.Add(new SendProjectileByVelocityAbility(GetDirection,transform, _projectilePrefab,_projectileCooldown));
        }

        private void Update()
        {
            (_activeAbilities.FirstOrDefault(x => x is BlinkForwardAbility) as BlinkForwardAbility).Range = _blinkDistance;
            (_activeAbilities.FirstOrDefault(x => x is SendProjectileByVelocityAbility) as SendProjectileByVelocityAbility).Speed = _projectileSpeed;
            
        }

        private Vector2 GetDirection()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}