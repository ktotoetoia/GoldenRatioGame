using UnityEngine;

namespace IM.Entities
{
    public class InitializeSingleEntity : MonoBehaviour
    {
        [SerializeField] private GameObject _toInitialize;
        [SerializeField] private Vector3 _startingPosition;
        private IGameObjectFactory _gameObjectFactory;
        
        public IEntity CreatedEntity { get; private set; }
        
        private void Awake()
        {
            _gameObjectFactory = GetComponent<IGameObjectFactory>();
            
            CreatedEntity = _gameObjectFactory.Create(_toInitialize).GetComponent<IEntity>();
            CreatedEntity.GameObject.transform.position = _startingPosition;
        }
    }
}