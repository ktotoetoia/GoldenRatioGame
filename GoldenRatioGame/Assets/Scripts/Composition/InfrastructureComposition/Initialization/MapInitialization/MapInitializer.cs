using IM.LifeCycle;
using IM.Map;
using IM.Map.Grid;
using IM.SaveSystem;
using IM.Values;
using UnityEngine;

namespace IM
{
   [CreateAssetMenu(fileName = "MapInitializer", menuName = "Initialization/Map Initializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private MapFactory _mapFactory;
        [SerializeField] private GameObject _floorPrefab;
        [SerializeField] private CappedValue<int> _cappedValue = new (3,8);

        public override void OnSceneLoaded(GameObject initializerGo, IGameObjectFactory factory)
        {
            Floor floor = factory.Create(_floorPrefab, false).GetComponent<Floor>();
            floor.Seed = Random.Range(-1000000, 1000000);
            floor.MinRooms = _cappedValue.MinValue;
            floor.MaxRooms = _cappedValue.MaxValue;
            
            floor.SetMapFactory(_mapFactory);
            floor.Next();
        }
    }
}