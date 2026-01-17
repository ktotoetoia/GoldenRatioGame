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
        
        private ModulePortSetup PortSetup => _portSetup ??= GetComponent<ModulePortSetup>();
        public IIcon Icon => _icon;
        
        public IModuleVisualObject CreateModuleVisualObject()
        {
            ModuleVisualObject moduleVisualObject = Instantiate(_visualPrefab,transform).GetComponent<ModuleVisualObject>();
            moduleVisualObject.Owner = GetComponent<IModule>();
            moduleVisualObject.AnimationChanges = GetComponents<IAnimationChange>();
            
            foreach ((IPort port, PortInfo portInfo) in PortSetup.PortsInfos)
            {
                IHierarchyTransform portTransform =  new HierarchyTransform();
                
                portTransform.LocalPosition = portInfo.Position;
                portTransform.LocalScale = Vector3.one;
                portTransform.LocalRotation = Quaternion.Euler(0f, 0f, portInfo.EulerZRotation);
                
                moduleVisualObject.AddPort(new PortVisualObject(moduleVisualObject,port, portTransform));
            }
            
            return moduleVisualObject;
        }
    }
}