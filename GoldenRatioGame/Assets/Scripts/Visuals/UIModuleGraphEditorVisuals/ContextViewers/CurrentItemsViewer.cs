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
        [SerializeField] private List<StyleSheet> _styleSheets = new ();
        [SerializeField] private string _containerName = "ItemsContainer";
        private UIDocument _document;
        private VisualElement _container;
        private IModuleEditingContext _context;
        private AbilityPoolEditingService _abilityPoolEditingService;
        private IWeaponEditingService _weaponEditingService;
        private CollectionDiffer<IDataModule<IExtensibleItem>> _differ;
        private readonly Dictionary<IDataModule<IExtensibleItem>, ItemStatsInfoElement> _itemDisplays = new();
        private readonly List<IStatPreviewer> _statPreviewers = new();
        private IAugmentPreviewer _augmentPreviewer;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.visible = false;
            _container = _document.rootVisualElement.Q<VisualElement>(_containerName);
            _augmentPreviewer = GetComponent<IAugmentPreviewer>();
            GetComponents(_statPreviewers);
            _statPreviewers.Remove(_augmentPreviewer);
        }

        private void Update()
        {
            if (_context == null) return;

            _differ?.Update(_context.GraphEditing.GraphReadOnly.DataModules);

            foreach (ItemStatsInfoElement element in _itemDisplays.Values)
            {
                (element.Action as ExtensibleItemExtra)?.Update();
                element.UpdatePreviews();
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
                    ItemStatsInfoElement element = CreateItemDisplay(module);
                    _itemDisplays[module] = element;
                    _container.Add(element);
                },
                module =>
                {
                    if (_itemDisplays.Remove(module, out var element))
                        _container.Remove(element);
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

        private ItemStatsInfoElement CreateItemDisplay(IDataModule<IExtensibleItem> module)
        {
            var element = new ItemStatsInfoElement(_statPreviewers, _augmentPreviewer);
            element.SetItem(module.Value);
            element.SetAction(new ExtensibleItemExtra(
                module.Value,
                weapon => _weaponEditingService.ClearWeapon(weapon),
                _abilityPoolEditingService
            ));

            foreach (StyleSheet styleSheet in _styleSheets)
            {
                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
    }
}