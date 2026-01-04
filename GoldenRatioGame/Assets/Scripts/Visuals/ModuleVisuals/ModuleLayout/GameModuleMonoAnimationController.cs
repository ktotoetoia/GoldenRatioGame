using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class GameModuleMonoAnimationController : MonoBehaviour, IModuleAnimationController
    {
        [SerializeField] private GameObject _visualPrefab;
        private readonly List<IAnimationChange>  _animationChange = new();
        private Dictionary<IPort,ITransformPort> _visualPorts;
        private ModulePortSetup _portSetup;
        
        public IAnimationModule ReferenceModule { get; private set; }

        private void Awake()
        {
            _portSetup = GetComponent<ModulePortSetup>();
            GetComponents(_animationChange);
        }

        private void Update()
        {
            if(ReferenceModule == null) return;

            foreach (IAnimationChange animationChange in _animationChange)
            {
                animationChange.ApplyToAnimator(ReferenceModule.Animator);
            }
        }

        public ITransformPort GetReferencePort(IPort port)
        {
            return _visualPorts[port];
        }

        public IAnimationModule CreateNewReferenceModule()
        {
            ReferenceModule?.Dispose();

            return ReferenceModule = CreateAnimationModuleCopy(_visualPorts = new());
        }

        public IAnimationModule CreateAnimationModuleCopy(IDictionary<IPort, ITransformPort> visualPortMap)
        {
            AnimationModule animationModule = Instantiate(_visualPrefab).GetComponent<AnimationModule>();

            foreach ((IPort port, PortInfo portInfo) in _portSetup.PortsInfos)
            {
                IHierarchyTransform transform = new HierarchyTransform();
                ITransformPort transformPort = new TransformPort(animationModule, transform);
                
                (animationModule.HierarchyTransform as IHierarchyTransform).AddChild(transform);

                transform.LocalPosition = portInfo.Position;
                transform.LocalScale = Vector3.one;
                transform.LocalRotation = Quaternion.LookRotation(portInfo.Normal, Vector3.up);
                
                animationModule.AddPort(transformPort);
                visualPortMap[port] = transformPort;
            }

            return animationModule;
        }
    }
}