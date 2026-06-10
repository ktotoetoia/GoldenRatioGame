using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;
using IM.LifeCycle;
using IM.Modules;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class CurrentItemsViewer : ContextViewer
    {
        [SerializeField] private string _containerName = "ItemsContainer";
        private UIDocument _document;
        private VisualElement _container;
        private IModuleEditingContext _context;
        private AbilityPoolEditingService _abilityPoolEditingService;
        private IWeaponEditingService _weaponEditingService;
        private CollectionDiffer<IDataModule<IExtensibleItem>> _differ;
        private readonly Dictionary<IDataModule<IExtensibleItem>, ItemDisplayContainer> _itemDisplayContainers = new();

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
            _container = _document.rootVisualElement.Q<VisualElement>(_containerName);
        }

        private void Update()
        {
            if (_context == null) return;

            _differ?.Update(_context.GraphEditing.GraphReadOnly.DataModules);

            foreach (var container in _itemDisplayContainers.Values)
                (container.ExtraElement as ExtensibleItemExtra)?.Update();
        }

        public override void SetContext(IModuleEditingContext context)
        {
            _context = context;
            _abilityPoolEditingService = context.Services.Get<AbilityPoolEditingService>();
            _weaponEditingService = context.Services.Get<IWeaponEditingService>();
            _document.rootVisualElement.visible = true;

            _differ = new CollectionDiffer<IDataModule<IExtensibleItem>>(
                module =>
                {
                    var container = CreateItemContainer(module);
                    _itemDisplayContainers[module] = container;
                    _container.Add(container);
                },
                module =>
                {
                    if (_itemDisplayContainers.Remove(module, out var container))
                        _container.Remove(container);
                }
            );
        }

        public override void ClearContext()
        {
            _context = null;
            _abilityPoolEditingService = null;
            _weaponEditingService = null;
            _document.rootVisualElement.visible = false;

            _container?.Clear();
            _itemDisplayContainers.Clear();
        }

        public IAbilityContainer GetContainerAt(Vector3 position)
        {
            List<ItemDisplayContainer> elements = WorldDocumentUtility.GetElementsAtPosition<ItemDisplayContainer>(_document,position).ToList();
            
            foreach (ItemDisplayContainer element in elements)
            {
                if(element.ExtraElement is ExtensibleItemExtra { AbilityContainer: not null } container)
                {
                    return container.AbilityContainer;
                }
            }
            return null;
        }

        private ItemDisplayContainer CreateItemContainer(IDataModule<IExtensibleItem> module)
        {
            var container = new ItemDisplayContainer();
            container.SetItem(module.Value);
            container.SetExtraElement(new ExtensibleItemExtra(
                module.Value,
                weapon => _weaponEditingService.ClearWeapon(weapon),
                _abilityPoolEditingService
            ));
            return container;
        }
    }
}