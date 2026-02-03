using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public class GameModuleMonoVisual : MonoBehaviour, IModuleVisual
    {
        [SerializeField] private GameObject _inGamePrefab;
        [SerializeField] private GameObject _editorPrefab;
        private IObjectPool<IModuleVisualObject> _pool;
        
        public IObjectPool<IModuleVisualObject> EditorPool { get; private set; }
        public IObjectPool<IAnimatedModuleVisualObject> GamePool { get; private set; }

        private void Awake()
        {
            GamePool = new ObjectPool<IAnimatedModuleVisualObject>(
                () => Create(_inGamePrefab),
                OnGet,
                OnRelease,
                OnDestroyPooledObject
            );
            
            EditorPool= new ObjectPool<IModuleVisualObject>(
                () => Create(_editorPrefab),
                OnGet,
                OnRelease,
                OnDestroyPooledObject
            ); 
        }
        
        private IAnimatedModuleVisualObject Create(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, transform);
            ModuleVisualObject visual = go.GetComponent<ModuleVisualObject>();

            visual.Owner = GetComponent<IExtensibleModule>();
            visual.AnimationChanges = GetComponents<IAnimationChange>();
            visual.OnInitializationFinished();
            
            return visual;
        }

        private static void OnGet(IModuleVisualObject visual) => visual.OnGet();
        private static void OnRelease(IModuleVisualObject visual) => visual.OnRelease();
        private static void OnDestroyPooledObject(IModuleVisualObject visual) => visual.Dispose();
    }
}