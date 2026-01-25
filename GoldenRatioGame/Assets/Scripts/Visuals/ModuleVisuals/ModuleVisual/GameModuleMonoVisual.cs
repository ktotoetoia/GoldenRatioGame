using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Visuals
{
    [RequireComponent(typeof(ModulePortSetup))]
    public class GameModuleMonoVisual : MonoBehaviour, IModuleVisual
    {
        [SerializeField] private GameObject _visualPrefab;

        private ModulePortSetup _portSetup;
        private IObjectPool<IModuleVisualObject> _pool;
        private ModulePortSetup PortSetup => _portSetup ??= GetComponent<ModulePortSetup>();

        private void Awake()
        {
            _pool = new ObjectPool<IModuleVisualObject>(
                Create,
                OnGet,
                OnRelease,
                OnDestroyPooledObject
            );
        }
        
        public int CountInactive => _pool.CountInactive;
        public IModuleVisualObject Get() => _pool.Get();
        public PooledObject<IModuleVisualObject> Get(out IModuleVisualObject v) => _pool.Get(out v);
        public void Release(IModuleVisualObject element)=> _pool.Release(element);
        public void Clear() => _pool.Clear();

        private IModuleVisualObject Create()
        {
            var go = Instantiate(_visualPrefab, transform);
            var visual = go.GetComponent<ModuleVisualObject>();

            visual.Owner = GetComponent<IExtensibleModule>();
            visual.AnimationChanges = GetComponents<IAnimationChange>();

            foreach ((IPort port, PortInfo portInfo) in PortSetup.PortsInfos)
            {
                IHierarchyTransform portTransform = new HierarchyTransform
                {
                    LocalPosition = portInfo.Position,
                    LocalScale = Vector3.one,
                    LocalRotation = Quaternion.Euler(0f, 0f, portInfo.EulerZRotation)
                };

                visual.AddPort(
                    new PortVisualObject(visual, port, portTransform),
                    portTransform
                );
            }

            visual.Visibility = false;
            
            return visual;
        }

        private static void OnGet(IModuleVisualObject visual) => visual.Visibility = true;
        private static void OnRelease(IModuleVisualObject visual)
        {
            visual.Reset();
            visual.Visibility = false;
        }
        private static void OnDestroyPooledObject(IModuleVisualObject visual) => visual.Dispose();
    }
}