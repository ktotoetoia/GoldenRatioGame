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
                SetRotation(rotationInfo, direction);
            }
        }

        private void SetRotation(RotationInfo rotationInfo, HorizontalDirection direction)
        {
            if(_moduleVisualObject.PortsVisualObjects.Count <= rotationInfo.PortID) return;
            float rotation = GetRotation(rotationInfo, direction);

            _moduleVisualObject.PortsVisualObjects[rotationInfo.PortID].Transform.LocalRotation = Quaternion.Euler(0, 0, rotation);
        }

        private float GetRotation(RotationInfo rotationInfo, HorizontalDirection direction)
        {
            switch(direction)
            {
                case HorizontalDirection.Left:
                    return rotationInfo.LeftRotation;
                case HorizontalDirection.Right:
                    return rotationInfo.RightRotation;
                default:
                    return rotationInfo.DefaultRotation;
            }
        }
        
        [Serializable]
        private class RotationInfo
        {
            [field: SerializeField] public int PortID { get; set; }
            [field: SerializeField] public int DefaultRotation { get; set; }
            [field: SerializeField] public float LeftRotation { get; set; }
            [field: SerializeField] public float RightRotation { get; set; }
        }
    }
}