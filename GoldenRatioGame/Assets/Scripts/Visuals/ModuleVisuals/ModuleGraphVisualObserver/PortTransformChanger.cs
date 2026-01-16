using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class PortTransformChanger : MonoBehaviour, IPortTransformChanger
    {
        [SerializeField] private int _port;
        [SerializeField] private Vector2 _newPosition;
        private IModuleVisual _moduleVisual;
        
        public IModuleGraphStructureUpdater ModuleGraphStructureUpdater { get; set; }

        private void Awake()
        {
            _moduleVisual = GetComponent<IModuleVisual>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                IPort port = _moduleVisual.ModuleVisualObject.Owner.Ports.ElementAt(_port);
                _moduleVisual.ModuleVisualObject.PortsTransforms[port].LocalPosition = _newPosition;
                ModuleGraphStructureUpdater.OnPortTransformChanged(port);
            }
        }
    }
}