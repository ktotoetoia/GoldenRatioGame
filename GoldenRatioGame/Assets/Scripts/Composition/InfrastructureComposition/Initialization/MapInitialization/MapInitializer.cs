using IM.LifeCycle;
using IM.Map.Grid;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
   [CreateAssetMenu(fileName = "MapInitializer", menuName = "Initialization/Map Initializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private MapGenerator _mapGenerator;

        public override void OnSceneLoaded(GameObject initializerGo, IGameObjectFactory factory)
        {
            _mapGenerator.Create(factory,Random.Range(3,8),Random.Range(-1000000,1000000));
        }
    }
}