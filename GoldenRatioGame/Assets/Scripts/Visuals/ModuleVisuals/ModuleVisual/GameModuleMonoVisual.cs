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
        
        private IObjectPool<IModuleVisualObject> Pool => _pool ??=new ObjectPool<IModuleVisualObject>(
            Create,
            OnGet,
            OnRelease,
            OnDestroyPooledObject
        );
        
        private ModulePortSetup PortSetup => _portSetup ??= GetComponent<ModulePortSetup>();
        
        public int CountInactive => Pool.CountInactive;
        public IModuleVisualObject Get() => Pool.Get();
        public PooledObject<IModuleVisualObject> Get(out IModuleVisualObject v) => Pool.Get(out v);
        public void Release(IModuleVisualObject element)=> Pool.Release(element);
        public void Clear() => Pool.Clear();

        private IModuleVisualObject Create()
        {
            GameObject go = Instantiate(_visualPrefab, transform);
            ModuleVisualObject visual = go.GetComponent<ModuleVisualObject>();

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

            return visual;
        }

        private static void OnGet(IModuleVisualObject visual) => visual.Visibility = true;
        private static void OnRelease(IModuleVisualObject visual) => visual.Reset();
        private static void OnDestroyPooledObject(IModuleVisualObject visual) => visual.Dispose();
    }
}