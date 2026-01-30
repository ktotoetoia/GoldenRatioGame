using UnityEngine;

namespace IM.Entities
{
    public class InitializePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _startingPosition;
        [SerializeField] private GameObject _requirePlayer;
        
        private void Awake()
        {
            IGameObjectFactory gameObjectFactory = GetComponent<IGameObjectFactory>();
            IEntity createdEntity = gameObjectFactory.Create(_playerPrefab).GetComponent<IEntity>();

            foreach (IRequirePlayerEntity requirePlayerEntity in _requirePlayer.GetComponents<IRequirePlayerEntity>())
            {
                requirePlayerEntity.SetPlayerEntity(createdEntity);
            }
            
            createdEntity.GameObject.transform.position = _startingPosition;
        }
    }
}