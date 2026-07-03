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
        [SerializeField] private bool _stop;
        private UIDocument _document;
        private VisualElement _container;
        private IModuleEditingContext _context;
        private AbilityPoolEditingService _abilityPoolEditingService;
        private IWeaponEditingService _weaponEditingService;
        private CollectionDiffer<IDataModule<IExtensibleItem>> _differ;
        private readonly Dictionary<IDataModule<IExtensibleItem>, ItemDisplay> _itemDisplays = new();
        private readonly List<IStatPreviewer> _statPreviewers = new();

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
            _container = _document.rootVisualElement.Q<VisualElement>(_containerName);
            GetComponents(_statPreviewers);
        }

        private void Update()
        {
            if (_context == null) return;

            _differ?.Update(_context.GraphEditing.GraphReadOnly.DataModules);

            foreach (ItemDisplay display in _itemDisplays.Values)
            {
                if(!_stop) (display.Element.Action as ExtensibleItemExtra)?.Update();

                object item = display.Module.Value;

                foreach (var (previewer, element) in display.StatElements)
                    previewer.UpdatePreview(element, item);
            }
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
                    ItemDisplay display = CreateItemDisplay(module);
                    _itemDisplays[module] = display;
                    _container.Add(display.Element);
                },
                module =>
                {
                    if (_itemDisplays.Remove(module, out ItemDisplay display))
                        _container.Remove(display.Element);
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
            _itemDisplays.Clear();
        }

        public IAbilityContainer GetContainerAt(Vector3 position)
        {
            List<ItemInfoElement> elements = WorldDocumentUtility.GetElementsAtPosition<ItemInfoElement>(_document, position).ToList();

            foreach (ItemInfoElement element in elements)
            {
                if (element.Action is ExtensibleItemExtra { AbilityContainer: not null } container)
                    return container.AbilityContainer;
            }

            return null;
        }

        private ItemDisplay CreateItemDisplay(IDataModule<IExtensibleItem> module)
        {
            var element = new ItemInfoElement();
            element.SetItem(module.Value);
            element.SetAction(new ExtensibleItemExtra(
                module.Value,
                weapon => _weaponEditingService.ClearWeapon(weapon),
                _abilityPoolEditingService
            ));

            var statContainer = new VisualElement();
            var statElements = new Dictionary<IStatPreviewer, VisualElement>();

            foreach (IStatPreviewer statPreviewer in _statPreviewers)
            {
                VisualElement statElement = statPreviewer.GetPreview(module.Value);

                if (statElement == null) continue;

                statContainer.Add(statElement);
                statElements[statPreviewer] = statElement;
            }

            element.SetAdditionalInfo(statContainer);

            return new ItemDisplay
            {
                Element = element,
                Module = module,
                StatElements = statElements
            };
        }

        private class ItemDisplay
        {
            public ItemInfoElement Element;
            public IDataModule<IExtensibleItem> Module;
            public Dictionary<IStatPreviewer, VisualElement> StatElements;
        }
    }
}