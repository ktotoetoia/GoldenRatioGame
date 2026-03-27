using IM.Entities;
using IM.Health;
using UnityEngine;

namespace IM
{
    public class DestroyEntityHealthContract : MonoBehaviour
    {
        [SerializeField] private float _minHealth;
        private IEntity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<IEntity>();
            GetComponent<IFloatHealthEvents>().OnHealthChanged += CheckHealth;
        }

        public void CheckHealth(float value)
        {
            if( value<= _minHealth) _entity.Destroy();
        }
    }
}