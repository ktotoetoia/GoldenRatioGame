using System.Collections.Generic;
using IM.Common;
using UnityEngine;

namespace IM.Entities
{
    public class ExtractPlayerByTag : MonoBehaviour, IGameObjectFactoryObserver
    {
        [SerializeField] private Vector3 _startingPosition;
        [SerializeField] private List<GameObject> _requirePlayer;
        [SerializeField] private string _tag;
        private IEntity _player;
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            if (instance.tag.Equals(_tag))
            {
                InitializePlayer(instance);
            }
        }

        private void InitializePlayer(GameObject player)
        {
            if (!player.TryGetComponent(out IEntity playerEntity))
            {
                Debug.LogWarning("Non entity GameObject is tagged as a Player, only player entity should be tagged as Player");
                
                return;
            }

            if (_player != null)
            {
                Debug.LogWarning("Trying to add a new entity that is tagged as a Player, only one entity can be a player");

                return;
            }

            _player = playerEntity;
            _player.GameObject.transform.position = _startingPosition;
            
            foreach (GameObject requirePlayer in  _requirePlayer)
            {
                foreach (IRequirePlayerEntity requirePlayerEntity in requirePlayer.GetComponents<IRequirePlayerEntity>())
                {
                    requirePlayerEntity.SetPlayerEntity(_player);
                }   
            }
        }
    }
}