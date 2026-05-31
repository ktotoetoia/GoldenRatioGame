using System.Collections;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.EntityIntelligence
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class AutoNavMeshBaker : MonoBehaviour
    {
        [SerializeField] private Transform _mapRoot;
        [SerializeField] private float _boundsPadding = 2f;
        [SerializeField] private float _volumeDepth = 10f;

        private NavMeshSurface _surface;

        private void Awake() => _surface = GetComponent<NavMeshSurface>();

        private void OnDisable()
        {
            if (_surface?.navMeshData != null)
                _surface.RemoveData();
        }
        private void OnEnable()
        {
            if (_mapRoot != null)
                StartCoroutine(BakeNextFrame());
        }

        private IEnumerator BakeNextFrame()
        {
            yield return null;
            if (isActiveAndEnabled) Bake();
        }
        
        public void BakeFor(Transform root)
        {
            _mapRoot = root;
            StartCoroutine(BakeNextFrame());
        }

        [ContextMenu("Bake")]
        private void Bake()
        {
            if (!_surface) _surface = GetComponent<NavMeshSurface>();
            if (_mapRoot == null) return;

            Bounds worldBounds = CalculateMapBounds();

            _surface.collectObjects = CollectObjects.Volume;
            _surface.center = _surface.transform.InverseTransformPoint(worldBounds.center);

            Vector3 volumeSize = worldBounds.size + Vector3.one * _boundsPadding;
            volumeSize.z = Mathf.Max(volumeSize.z, _volumeDepth);
            _surface.size = volumeSize;

            _surface.BuildNavMesh();
        }

        private Bounds CalculateMapBounds()
        {
            Tilemap[] tilemaps = _mapRoot.GetComponentsInChildren<Tilemap>();

            if (tilemaps.Length == 0)
                return new Bounds(_mapRoot.position, Vector3.one * 200f);

            bool initialized = false;
            Bounds result = default;

            foreach (Tilemap tilemap in tilemaps)
            {
                tilemap.CompressBounds();
                if (tilemap.localBounds.size == Vector3.zero) continue;

                Bounds world = new(
                    tilemap.transform.TransformPoint(tilemap.localBounds.center),
                    tilemap.localBounds.size);

                if (!initialized)
                {
                    result = world;
                    initialized = true;
                }
                else result.Encapsulate(world);
            }

            return initialized ? result : new Bounds(_mapRoot.position, Vector3.one * 200f);
        }
    }
}