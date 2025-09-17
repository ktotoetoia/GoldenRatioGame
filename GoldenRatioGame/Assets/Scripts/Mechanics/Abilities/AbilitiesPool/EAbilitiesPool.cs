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
        private readonly List<IAbility> _abilities = new();
        public IEnumerable<IAbility> Abilities => _abilities;

        private void Awake()
        {
            _abilities.Add(new BlinkForwardAbility(GetDirection,GetComponent<Rigidbody2D>(),_blinkCooldown));
            _abilities.Add(new SendProjectileByVelocityAbility(GetDirection,transform, _projectilePrefab,_projectileCooldown));
        }

        private void Update()
        {
            (_abilities.FirstOrDefault(x => x is BlinkForwardAbility) as BlinkForwardAbility).Range = _blinkDistance;
            (_abilities.FirstOrDefault(x => x is SendProjectileByVelocityAbility) as SendProjectileByVelocityAbility).Speed = _projectileSpeed;
            
        }

        private Vector2 GetDirection()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}