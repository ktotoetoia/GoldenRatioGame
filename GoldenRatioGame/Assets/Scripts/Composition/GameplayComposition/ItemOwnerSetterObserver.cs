using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class ItemOwnerSetterObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private Transform _onAddedTransformIn;
        [SerializeField] private Transform _onRemovedTransformOut;
        [SerializeField] private GameObject _entitySource;
        private IEntity _entity;
        private CollectionDiffer<object> _storageEntities;

        private void Awake()
        {
            _entity = _entitySource.GetComponent<IEntity>();
            _storageEntities = new CollectionDiffer<object>(OnAdded, OnRemoved);
        }

        private void OnAdded(object obj)
        {
            if (obj is MonoBehaviour mb)
            {
                mb.transform.SetParent(_onAddedTransformIn);
                mb.transform.position = Vector3.zero;
            }

            (obj as IHaveOwner)?.SetOwner(_entity);
        }

        private void OnRemoved(object obj)
        {
            if (obj is MonoBehaviour mb && mb.transform.parent == _onAddedTransformIn)
                mb.transform.SetParent(_onRemovedTransformOut.parent);

            if (obj is IHaveOwner haveOwner && haveOwner.Owner == _entity)
                haveOwner.SetOwner(null);
        }

        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            UpdateStorageEntities(snapshot);
        }

        private void UpdateStorageEntities(IModuleEditingContextReadOnly snapshot)
        {
            if (snapshot == null) return;

            var items = new List<object>();
            items.AddRange(snapshot.Graph.DataModules.Select(x => x.Value));
            items.AddRange(snapshot.Storage.Select(x => x.Item));

            _storageEntities.Update(items);
        }
    }
}