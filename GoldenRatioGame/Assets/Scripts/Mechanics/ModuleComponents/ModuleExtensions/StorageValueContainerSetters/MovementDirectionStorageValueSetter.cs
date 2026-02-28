using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Events;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Values;

namespace IM.Visuals
{
    public class MovementDirectionStorageValueSetter : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private string _valueStorageTag = DirectionConstants.Focus;
        private List<IValueStorageContainer> _containers;
        private IVectorMovement _movement;
        
        private void Awake()
        {
            _movement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_movement.Direction == Vector2.zero || _containers == null) return;
            
            SetDirectionToContainers(DirectionUtils.GetEnumDirection(_movement.Direction));
        }
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if(graph == null) throw new ArgumentNullException(nameof(graph));

            _containers = graph.Modules.OfType<IExtensibleModule>().SelectMany(x=> x.Extensions.GetAll<IValueStorageContainer>()).ToList();
            
            SetDirectionToContainers(Direction.Right);
        }

        private void SetDirectionToContainers(Direction direction) =>
            _containers.ForEach(x => x.GetOrCreate<Direction>(_valueStorageTag).Value = direction);
    }
}