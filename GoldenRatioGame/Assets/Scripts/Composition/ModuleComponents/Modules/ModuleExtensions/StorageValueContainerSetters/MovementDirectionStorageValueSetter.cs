using System;
using System.Collections.Generic;
using System.Linq;
using IM.Events;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementDirectionStorageValueSetter : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
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
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            _containers = snapshot.Graph.DataModules.SelectMany(x=> x.Value.Extensions.GetAll<IValueStorageContainer>()).ToList();
            SetDirectionToContainers(Direction.Right);
        }

        private void SetDirectionToContainers(Direction direction) =>
            _containers.ForEach(x => x.GetOrCreate<Direction>(_valueStorageTag).Value = direction);
    }
}