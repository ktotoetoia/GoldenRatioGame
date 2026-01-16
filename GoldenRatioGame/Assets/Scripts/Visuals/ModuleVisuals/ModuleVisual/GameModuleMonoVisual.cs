using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(ModulePortSetup))]
    public class GameModuleMonoVisual : MonoBehaviour, IModuleVisual
    {
        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private Icon _icon;
        private ModulePortSetup _portSetup;
        
        public IModuleVisualObject ModuleVisualObject { get; private set; }
        public IIcon Icon => _icon;

        private void Awake()
        {
            _portSetup = GetComponent<ModulePortSetup>();
        }
        
        public void ResetReferenceModule()
        {
            ModuleVisualObject?.Dispose();

            ModuleVisualObject = CreateAnimationModuleCopy();
        }

        private ModuleVisualObject CreateAnimationModuleCopy()
        {
            ModuleVisualObject moduleVisualObject = Instantiate(_visualPrefab,transform).GetComponent<ModuleVisualObject>();
            moduleVisualObject.Owner = GetComponent<IModule>();
            
            foreach ((IPort port, PortInfo portInfo) in _portSetup.PortsInfos)
            {
                IHierarchyTransform portTransform =  new HierarchyTransform();
                
                portTransform.LocalPosition = portInfo.Position;
                portTransform.LocalScale = Vector3.one;
                portTransform.LocalRotation = Quaternion.Euler(0f, 0f, portInfo.EulerZRotation);
                
                moduleVisualObject.AddPort(port,portTransform);
            }
            
            return moduleVisualObject;
        }
    }
}