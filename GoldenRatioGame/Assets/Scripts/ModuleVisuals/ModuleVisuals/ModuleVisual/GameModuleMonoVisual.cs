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
        
        private IModuleVisualObject Create(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, transform);
            IModuleVisualObject visual = go.GetComponent<IModuleVisualObject>();
            
            visual.DefaultParent = transform;
            if(visual is IAnimatedModuleVisualObject a) a.AnimationChanges = GetComponents<IAnimationChange>();
            visual.FinishInitialization(GetComponent<IExtensibleModule>());
            
            return visual;
        }

        private static void OnGet(IModuleVisualObject visual) => visual.OnGet();
        private static void OnRelease(IModuleVisualObject visual) => visual.OnRelease();
        private static void OnDestroyPooledObject(IModuleVisualObject visual) => visual.Dispose();
    }
}