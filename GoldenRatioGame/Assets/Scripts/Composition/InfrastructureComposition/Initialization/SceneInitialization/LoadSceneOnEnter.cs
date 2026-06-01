using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace Tests
{
    public class LoadSceneOnEnter : MonoBehaviour
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private Location _location;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _gameInfoController.ProgressTo(_location);
            }
        }
    }
}