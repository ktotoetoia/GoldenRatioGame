using IM.Modules;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public class GameModuleMonoVisual : MonoBehaviour, IModuleVisual
    {
        [SerializeField] private GameObject _inGamePrefab;
        [SerializeField] private GameObject _editorPrefab;
        
        public IObjectPool<IModuleVisualObject> EditorPool { get; private set; }
        public IObjectPool<IModuleVisualObject> GamePool { get; private set; }

        private void Awake()
        {
            GamePool = new ObjectPool<IModuleVisualObject>(
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
            visual.DefaultParent = transform;
            
            return visual;
        }

        private static void OnGet(IModuleVisualObject visual) => visual.OnGet();
        private static void OnRelease(IModuleVisualObject visual) => visual.OnRelease();
        private static void OnDestroyPooledObject(IModuleVisualObject visual) => visual.Dispose();
    }
}