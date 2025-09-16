using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class EAbilitiesPool : MonoBehaviour, IAbilitiesPool
    {
        [SerializeField] private float _projectileCooldown;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _blinkDistance;
        [SerializeField] private float _blinkCooldown;
        private readonly List<IActiveAbility> _activeAbilities = new();
        public IEnumerable<IActiveAbility> ActiveAbilities => _activeAbilities;
        public IEnumerable<IAbility> Abilities => _activeAbilities;

        private void Awake()
        {
            _activeAbilities.Add(new BlinkForwardAbility(GetDirection, transform,_blinkCooldown));
            _activeAbilities.Add(new SendProjectileByVelocityAbility(GetDirection,transform, _projectilePrefab,_projectileCooldown));
        }
        
        private Vector2 GetDirection()
        {
            return ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position)).normalized * _blinkDistance;
        }
    }
}