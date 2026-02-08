using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Movement;
using TDS.Events;
using UnityEngine;

namespace IM.Modules
{
    public class MovementReflector : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private IVectorMovement _movement;
        private IEnumerable<IRequireMovement> _requireMovement;
        private IEnumerable<IValueStorageContainer> _containers;

        private void Awake()
        {
            _movement =  GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_movement.MovementDirection == Vector2.zero || _containers == null) return;
            
            MovementDirection dir = MovementDirection.None;

            if (_movement.MovementDirection.x > 0f) dir |= MovementDirection.Right;
            if (_movement.MovementDirection.x < 0f) dir |= MovementDirection.Left;
            if (_movement.MovementDirection.y > 0f) dir |= MovementDirection.Up;
            if (_movement.MovementDirection.y < 0f) dir |= MovementDirection.Down;

            foreach (IValueStorageContainer container in _containers)
            {
                container.GetOrCreate<MovementDirection>().Value = dir;
            }
        }

        private void FixedUpdate()
        {
            if(_requireMovement == null) return;

            foreach (IRequireMovement requireMovement in _requireMovement)
            {
                requireMovement.UpdateCurrentVelocity(_movement.MovementDirection);
            }
        }
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if(graph == null) throw new ArgumentNullException(nameof(graph));

            _requireMovement = graph.Modules.Where(x => x is IExtensibleModule).SelectMany(x => (x as IExtensibleModule).Extensions.GetExtensions<IRequireMovement>()).ToList();
            _containers = graph.Modules.Where(x=> x is IExtensibleModule).SelectMany(x=> (x as IExtensibleModule).Extensions.GetExtensions<IValueStorageContainer>()).ToList();

            foreach (IValueStorageContainer container in _containers)
            {
                container.GetOrCreate<MovementDirection>().Value = MovementDirection.Right;
            }
        }
    }
}