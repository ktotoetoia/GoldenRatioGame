using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.LifeCycle;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace IM.Modules
{
    public class FollowPlayerSmooth : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private List<Transform> _transforms;
        [SerializeField] private float _smoothTime = 0.2f;
        [SerializeField] private Vector3 _offset = new(0f, 0f, -10f);
        private Transform _target;
        private Vector3 _velocity;

        public void SetPlayerEntity(IEntity playerEntity)
        {
            _target = playerEntity.GameObject.transform;

            foreach (var cam in _transforms)
            {
                cam.position = _target.position + _offset;
            }
        }

        private void Awake()
        {
            if(!_target) return;
            
            foreach (var cam in _transforms)
            {
                cam.position = _target.position + _offset;
            }
        }

        private void LateUpdate()
        {
            if (!_target || _transforms.Count == 0) return;
            
            
            Vector3 targetPosition = _target.position + _offset;
            Vector3 position = Vector3.SmoothDamp(
                _transforms.First().position,
                targetPosition,
                ref _velocity,
                _smoothTime);
            
            foreach (var cam in _transforms)
            {
                cam.position = position;
            }
        }
    }
}