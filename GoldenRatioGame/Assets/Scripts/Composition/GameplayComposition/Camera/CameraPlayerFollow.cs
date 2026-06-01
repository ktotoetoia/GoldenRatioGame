using System.Collections.Generic;
using IM.Entities;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class CameraPlayerFollow : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private List<Camera> _cameras;
        [SerializeField] private float _smoothTime = 0.2f;
        [SerializeField] private Vector3 _offset = new(0f, 0f, -10f);
        private Transform _target;
        private Vector3 _velocity;

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _target = playerEntity.GameObject.transform;

            foreach (var camera in _cameras)
            {
                camera.transform.position = _target.position + _offset;
            }
            
        }

        private void Awake()
        {
            if(!_target) return;
            
            foreach (var camera in _cameras)
            {
                camera.transform.position = _target.position + _offset;
            }
        }

        private void LateUpdate()
        {
            if (!_target) return;

            Vector3 targetPosition = _target.position + _offset;

            foreach (var camera in _cameras)
            {
                camera.transform.position = Vector3.SmoothDamp(
                    camera.transform.position,
                    targetPosition,
                    ref _velocity,
                    _smoothTime);
            }
        }
    }
}