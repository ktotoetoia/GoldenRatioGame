using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Movement;
using UnityEngine;

namespace IM.Modules
{
    public class MovementReflector : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private GameObject _vectorMovementSource;
        private IVectorMovement _movement;
        private IEnumerable<IRequireMovement> _requireMovement;
        
        private void Awake()
        {
            _movement =  _vectorMovementSource.GetComponent<IVectorMovement>();
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

            _requireMovement = graph.Modules.OfType<IExtensibleModule>().SelectMany(x => x.Extensions.GetExtensions<IRequireMovement>()).ToList();
        }
    }
}