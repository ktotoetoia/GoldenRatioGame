using System.Collections.Generic;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(menuName = "Initialization/Object Initializer")]
    public class RandomPositionObjectInitializer : SceneInitializer
    {
        [SerializeField] private Bounds _bounds;
        [SerializeField] private List<GameObject> _toInitialize;
        
        public override void OnSceneLoaded(GameObject initializerGO, IGameObjectFactory factory)
        {
            foreach (GameObject obj in _toInitialize)
            {
                GameObject go = factory.Create(obj,false);

                go.transform.position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), Random.Range(_bounds.min.y, _bounds.max.y));
            }
        }
    }
}