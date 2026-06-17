using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using IM.Entities;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class NotifyOnAugmentAdded : MonoBehaviour, IRequirePlayerEntity
    {
        [SerializeField] private List<StyleSheet> _elementStyleSheets;
        private NotificationQueueManager _notificationQueueManager;
        
        private IAugmentContainer _playerAugments;
        private CollectionDiffer<IAugment> _differ;

        private bool _isPrimed;

        private void Awake()
        {
            _notificationQueueManager = GetComponent<NotificationQueueManager>();

            _differ = new CollectionDiffer<IAugment>(OnAugmentAdded, null);
        }

        public void SetPlayerEntity(IEntity playerEntity)
        {
            if (playerEntity != null && playerEntity.GameObject.TryGetComponent(out IAugmentContainer container))
            {
                _playerAugments = container;
                _isPrimed = false;
            }
        }

        private void Update()
        {
            if (_playerAugments != null)
            {
                _differ.Update(_playerAugments.Augments);

                _isPrimed = true; 
            }
        }

        private void OnAugmentAdded(IAugment augment)
        {
            if (!_isPrimed) return;

            var tooltipElement = new TooltipInfoElement
            {
                ShowIcon = true,
                ShowName = true,
                ShowShortDescription = true,
                ShowDescription = false,
                ShowDivider = false
            };
            
            foreach (StyleSheet sheet in _elementStyleSheets.Where(x=>x)) tooltipElement.styleSheets.Add(sheet);
            
            tooltipElement.Bind(new TooltipInfoWrapper(augment.Name,augment.ShortDescription,augment.Description,augment.Icon.Sprite));
            _notificationQueueManager.Preview(tooltipElement);
        }
    }
}