using System.Collections.Generic;
using UnityEngine;

namespace IM.Entities
{
    public class InitializePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _startingPosition;
        [SerializeField] private List<GameObject> _requirePlayer;
        
        private void Awake()
        {
            IGameObjectFactory gameObjectFactory = GetComponent<IGameObjectFactory>();
            IEntity createdEntity = gameObjectFactory.Create(_playerPrefab).GetComponent<IEntity>();
            createdEntity.GameObject.transform.position = _startingPosition;
            
            foreach (GameObject requirePlayer in  _requirePlayer)
            {
                foreach (IRequirePlayerEntity requirePlayerEntity in requirePlayer.GetComponents<IRequirePlayerEntity>())
                {
                    requirePlayerEntity.SetPlayerEntity(createdEntity);
                }   
            }
        }
    }
}