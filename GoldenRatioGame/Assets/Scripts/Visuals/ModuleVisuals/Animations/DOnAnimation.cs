using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Visuals
{
    public class DOnAnimation : MonoBehaviour
    {
        [SerializeField] private float _range;
        [SerializeField] private float _length;
        [SerializeField] private bool _isWalking;
        [SerializeField] private float _degrees;
        private IModuleVisualObject _moduleVisualObject;
        private List<Vector3> _portsDefaultPosition;
        private List<Quaternion> _portsDefaultRotation;
        
        private void Awake()
        {
            _moduleVisualObject = GetComponent<IModuleVisualObject>();
            _portsDefaultPosition = _moduleVisualObject.PortsVisualObjects.Select(x => x.Transform.LocalPosition).ToList();
            _portsDefaultRotation = _moduleVisualObject.PortsVisualObjects.Select(x => x.Transform.LocalRotation).ToList();
        }

        private void Update()
        {
            if (!_isWalking)
            {
                for (int i = 0; i < _moduleVisualObject.PortsVisualObjects.Count; i++)
                {
                    IPortVisualObject portVisual = _moduleVisualObject.PortsVisualObjects[i];
                    
                    portVisual.Transform.LocalPosition = _portsDefaultPosition[i];
                    portVisual.Transform.LocalRotation = _portsDefaultRotation[i];
                    _moduleVisualObject.ModuleGraphStructureUpdater.OnPortTransformChanged(portVisual.Port);
                }
                
                return;
            }

            _moduleVisualObject.PortsVisualObjects[1].Transform.LocalPosition = _portsDefaultPosition[1] + GetTransition();
            _moduleVisualObject.PortsVisualObjects[1].Transform.LocalRotation =
                Quaternion.Euler(0, 0, _portsDefaultRotation[1].eulerAngles.z + GetRotation());
            _moduleVisualObject.ModuleGraphStructureUpdater.OnPortTransformChanged(_moduleVisualObject.PortsVisualObjects[1].Port);

            _moduleVisualObject.PortsVisualObjects[3].Transform.LocalPosition = _portsDefaultPosition[3] - GetTransition();
            _moduleVisualObject.PortsVisualObjects[3].Transform.LocalRotation =
                Quaternion.Euler(0, 0, _portsDefaultRotation[3].eulerAngles.z - GetRotation());
            _moduleVisualObject.ModuleGraphStructureUpdater.OnPortTransformChanged(_moduleVisualObject.PortsVisualObjects[3].Port);
            
            _moduleVisualObject.PortsVisualObjects[2].Transform.LocalPosition =_portsDefaultPosition[2] - GetTransition();
            _moduleVisualObject.PortsVisualObjects[2].Transform.LocalRotation =
                Quaternion.Euler(0, 0, _portsDefaultRotation[2].eulerAngles.z - GetRotation());
            _moduleVisualObject.ModuleGraphStructureUpdater.OnPortTransformChanged(_moduleVisualObject.PortsVisualObjects[2].Port);


            _moduleVisualObject.PortsVisualObjects[4].Transform.LocalRotation =
                Quaternion.Euler(0, 0, _portsDefaultRotation[4].eulerAngles.z + GetRotation());
            _moduleVisualObject.PortsVisualObjects[4].Transform.LocalPosition =_portsDefaultPosition[4] +GetTransition();
            _moduleVisualObject.ModuleGraphStructureUpdater.OnPortTransformChanged(_moduleVisualObject.PortsVisualObjects[4].Port);
        }

        private Vector3 GetTransition()
        {
            return new Vector3(_range  * Mathf.Sin(Time.time * (2f * Mathf.PI / _length)), 0);
        }

        private float GetRotation()
        {
            return _degrees* Mathf.Sin(Time.time * (2f * Mathf.PI / _length));
        }
    }
}