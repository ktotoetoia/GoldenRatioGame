using System;
using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [RequireComponent(typeof(HorizontalDirectionSetter))]
    public class HorizontalDirectionPortVisualRotationChange : MonoBehaviour,IHorizontalDirectionDependant
    {
        [SerializeField] private List<RotationInfo> _rotationInfos;
        private IModuleVisualObject _moduleVisualObject;
        
        private void Awake()
        {
            _moduleVisualObject = GetComponent<IModuleVisualObject>();
        }

        public void OnDirectionChanged(HorizontalDirection direction)
        {
            foreach (RotationInfo rotationInfo in _rotationInfos)
            {
                if(_moduleVisualObject.PortsVisualObjects.Count <= rotationInfo.PortID) continue;
                _moduleVisualObject.PortsVisualObjects[rotationInfo.PortID].Transform.LocalRotation = Quaternion.Euler(
                    0, 0, direction == HorizontalDirection.Left ? rotationInfo.LeftRotation : rotationInfo.RightRotation);
                _moduleVisualObject.ModuleGraphStructureUpdater?.OnPortTransformChanged(_moduleVisualObject.PortsVisualObjects[rotationInfo.PortID].Port);
            }
        }
        
        [Serializable]
        private class RotationInfo
        {
            [field: SerializeField] public int PortID { get; set; }
            [field: SerializeField] public float LeftRotation { get; set; }
            [field: SerializeField] public float RightRotation { get; set; }
        }
    }
}