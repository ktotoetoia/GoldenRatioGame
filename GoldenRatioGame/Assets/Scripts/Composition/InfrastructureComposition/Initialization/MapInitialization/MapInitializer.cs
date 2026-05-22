using IM.LifeCycle;
using IM.Map;
using IM.Map.Grid;
using IM.SaveSystem;
using UnityEngine;

namespace IM
{
   [CreateAssetMenu(fileName = "MapInitializer", menuName = "Initialization/Map Initializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private MapInfoFactory mapInfoFactory;
        [SerializeField] private GameObject _floorPrefab;

        public override void OnSceneLoaded(GameObject initializerGo, IGameObjectFactory factory)
        {
            Floor floor = factory.Create(_floorPrefab, false).GetComponent<Floor>();
            floor.Seed = Random.Range(-1000000, 1000000);
            
            floor.SetMapFactory(mapInfoFactory);
            floor.Next();
        }
    }
}