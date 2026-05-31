using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class RandomPropRoomDecorator : MonoBehaviour, IRoomDecorator
    {
        [Header("Props")]
        [SerializeField] private List<GameObject> _bigProps = new();
        [SerializeField] private List<GameObject> _smallProps = new();

        [Header("Prop Settings")]
        [SerializeField, Min(0)] private int _bigPropCount = 2;
        [SerializeField, Min(0)] private int _smallPropCount = 4;
        [SerializeField, Min(0)] private float _minPortClearance = 2f;
        [SerializeField, Min(0)] private float _minPropSpacing = 1.5f;
        [SerializeField, Min(0)] private float _edgeThreshold = 1.5f;
        [SerializeField, Min(0)] private float _edgeBias = 10f;
        [SerializeField, Range(0, 1)] private float _centerWeight = 0.05f;
        [SerializeField, Min(0)] private float _clusterRadius = 3f;
        [SerializeField, Min(0)] private float _clusterBias = 8f;
        [SerializeField, Min(0.1f)] private float _candidateStep = 1f;

        public void Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory)
        {
            if (shape == null) return;

            List<Vector2> portPositions = room.RoomPorts
                .OfType<MonoBehaviour>()
                .Select(mb => (Vector2)mb.transform.localPosition)
                .ToList();

            List<Vector2> candidates = GenerateCandidates(portPositions, shape.Metrics);
            List<Vector2> bigPropPositions = new();

            if (_bigProps != null && _bigProps.Count > 0)
            {
                for (int i = 0; i < _bigPropCount && candidates.Count > 0; i++)
                {
                    float[] weights = candidates.Select(c => GetBigPropWeight(c, shape.Metrics)).ToArray();
                    Vector2 pos = WeightedRandom(candidates, weights);
                    
                    GameObject prefabToSpawn = GetRandomProp(_bigProps);
                    if (prefabToSpawn)
                    {
                        SpawnProp(prefabToSpawn, pos, room, factory);
                        bigPropPositions.Add(pos);
                    }
                    
                    candidates.RemoveAll(c => Vector2.Distance(c, pos) < _minPropSpacing);
                }
            }

            if (_smallProps != null && _smallProps.Count > 0)
            {
                for (int i = 0; i < _smallPropCount && candidates.Count > 0; i++)
                {
                    float[] weights = candidates.Select(c => GetSmallPropWeight(c, bigPropPositions)).ToArray();
                    Vector2 pos = WeightedRandom(candidates, weights);
                    
                    GameObject prefabToSpawn = GetRandomProp(_smallProps);
                    if (prefabToSpawn)
                    {
                        SpawnProp(prefabToSpawn, pos, room, factory);
                    }
                    
                    candidates.RemoveAll(c => Vector2.Distance(c, pos) < _minPropSpacing);
                }
            }
        }
        
        private GameObject GetRandomProp(List<GameObject> props)
        {
            var validProps = props.Where(p => p).ToList();
            if (validProps.Count == 0) return null;

            int randomIndex = Random.Range(0, validProps.Count);
            return validProps[randomIndex];
        }

        private List<Vector2> GenerateCandidates(List<Vector2> portPositions, ShapeMetrics metrics)
        {
            List<Vector2> candidates = new();
            float half = _candidateStep * 0.5f;

            for (float x = metrics.RoomOriginX + half; x < metrics.RoomOriginX + metrics.TotalW; x += _candidateStep)
            {
                for (float y = metrics.RoomOriginY + half; y < metrics.RoomOriginY + metrics.TotalH; y += _candidateStep)
                {
                    Vector2 pos = new(x, y);
                    if (!metrics.IsInsideShape(pos)) continue;
                    if (portPositions.Any(pp => Vector2.Distance(pos, pp) < _minPortClearance)) continue;
                    candidates.Add(pos);
                }
            }

            return candidates;
        }

        private float GetBigPropWeight(Vector2 pos, ShapeMetrics metrics) => metrics.GetEdgeDistance(pos) <= _edgeThreshold ? _edgeBias : _centerWeight;

        private float GetSmallPropWeight(Vector2 pos, List<Vector2> bigProps)
        {
            if (bigProps.Count == 0) return 1f;
            return bigProps.Min(bp => Vector2.Distance(pos, bp)) <= _clusterRadius ? _clusterBias : 1f;
        }

        private void SpawnProp(GameObject prefab, Vector2 localPos, IGameObjectRoom room, IGameObjectFactory factory)
        {
            GameObject go = factory.Create(prefab, false);
            room.Add(go);
            go.transform.localPosition = new Vector3(localPos.x, localPos.y, 0f);
        }

        private static Vector2 WeightedRandom(List<Vector2> candidates, float[] weights)
        {
            float total = 0f;
            foreach (float w in weights) total += w;
            if (total <= 0f) return candidates[Random.Range(0, candidates.Count)];

            float roll = Random.value * total;
            float cumulative = 0f;
            for (int i = 0; i < candidates.Count; i++)
            {
                cumulative += weights[i];
                if (roll <= cumulative) return candidates[i];
            }
            return candidates[^1];
        }
    }
}