using System.Collections.Generic;
using System.Linq;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    [RequireComponent(typeof(ModuleEntity))]
    public class ModuleEntityRandomItemDrop : MonoBehaviour
    {
        [SerializeField] private int _dropCount = 2;
        private readonly List<IItemDropObserver> _itemDropObservers = new();
        private ModuleEntity _moduleEntity;

        private void Awake()
        {
            _moduleEntity = GetComponent<ModuleEntity>();
            _moduleEntity.Disassembled += OnDisassembled;
            GetComponents(_itemDropObservers);
        }

        private void OnDestroy()
        {
            if (_moduleEntity != null)
                _moduleEntity.Disassembled -= OnDisassembled;
        }

        private void OnDisassembled(IEnumerable<IExtensibleItem> modules)
        {
            var moduleList = modules.ToList();
            var remainList = new List<IExtensibleItem>();
            int itemsToKeep = Mathf.Min(_dropCount, moduleList.Count);
        
            for (int i = 0; i < itemsToKeep; i++)
            {
                int index = Random.Range(0, moduleList.Count);
                remainList.Add(moduleList[index]);
                moduleList.RemoveAt(index);
            }

            foreach (var module in moduleList)
            {
                module.Destroy();
            }

            foreach (IExtensibleItem item in remainList)
            {
                item.GameObject.transform.SetParent(transform.parent);
            }
            
            foreach (IItemDropObserver dropObserver in _itemDropObservers)
            {
                foreach (IExtensibleItem module in remainList)
                {
                    dropObserver.OnItemDropped(module);
                }
            }
        }
    }
}