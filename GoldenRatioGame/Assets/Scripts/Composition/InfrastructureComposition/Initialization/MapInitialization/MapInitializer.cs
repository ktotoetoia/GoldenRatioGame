using IM.LifeCycle;
using IM.Map.Grid;
using IM.SaveSystem;
using IM.Values;
using UnityEngine;

namespace IM
{
   [CreateAssetMenu(fileName = "MapInitializer", menuName = "Initialization/Map Initializer")]
    public class MapInitializer : SceneInitializer
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private CappedValue<int> _cappedValue = new (3,8);

        public override void OnSceneLoaded(GameObject initializerGo, IGameObjectFactory factory)
        {
            _mapGenerator.Create(factory,Random.Range(_cappedValue.MinValue,_cappedValue.MaxValue),Random.Range(-1000000,1000000));
        }
    }
}