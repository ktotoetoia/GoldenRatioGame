using IM.Abilities;
using IM.Common;
using IM.Items;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Modules
{
    public class FireProjectileAbilityExtension : MonoBehaviour, IAbilityExtension
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _cooldown = 3;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private Sprite _iconSprite;
        
        public IAbilityReadOnly Ability { get; private set; }

        private void Awake()
        {
            IObjectPool<GameObject> projectilePool = new ObjectPool<GameObject>(Create,OnGet, OnRelease,HandleDestroy);

            Ability =  new SendProjectileByVelocityAbility(projectilePool, GetComponent<IPositionProvider>(), _cooldown)
            {
                Speed = _projectileSpeed,
                Icon = new Icon(_iconSprite)
            };
        }

        private GameObject Create()
        {
            return Instantiate(_projectilePrefab,transform);
        }

        private void OnGet(GameObject go)
        {
            go.SetActive(true);
        }

        private void OnRelease(GameObject go)
        {
            go.SetActive(false);
        }

        private void HandleDestroy(GameObject go)
        {
            Destroy(go);
        }
    }
}