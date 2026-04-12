using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Movement;
using UnityEngine;

namespace IM.Modules
{
    public class MovementReflector : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _vectorMovementSource;
        private IVectorMovement _movement;
        private IEnumerable<IRequireMovement> _requireMovement;
        
        private void Awake()
        {
            _movement = _vectorMovementSource.GetComponent<IVectorMovement>();
        }

        private void FixedUpdate()
        {
            if(_requireMovement == null) return;

            foreach (IRequireMovement requireMovement in _requireMovement)
            {
                requireMovement.UpdateCurrentVelocity(_movement.Direction);
            }
        }
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            if(snapshot == null) throw new ArgumentNullException(nameof(snapshot));

            _requireMovement = snapshot.Graph.DataModules.SelectMany(x => x.Value.Extensions.GetAll<IRequireMovement>()).ToList();
        }
    }
}