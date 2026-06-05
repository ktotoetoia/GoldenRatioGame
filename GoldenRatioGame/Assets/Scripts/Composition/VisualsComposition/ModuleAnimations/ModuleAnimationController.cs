using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Events;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleAnimationController : MonoBehaviour
    {
        [SerializeField] private EntityModuleGraphVisualObserver _observer;
        [SerializeField] private ControllableCurveMovement _movement;
        private IAbilityUserEvents _abilityUserEvents;
        private IAbilityUser<IAbilityPoolReadOnly> _abilityUser;

        private void Awake()
        {
            _abilityUserEvents = GetComponent<IAbilityUserEvents>();
            _abilityUserEvents.AbilityStarted += OnAbilityUsed;
            _abilityUser = GetComponent<IAbilityUser<IAbilityPoolReadOnly>>();
        }
        
        private void Update()
        {
            Move(_movement.Direction);
            
            BoolAnimationContext c = new BoolAnimationContext("Stunned",_abilityUser.IsInterrupted);

            foreach (IModuleAnimator moduleAnimator in Animators)
            {
                if(moduleAnimator.CanPlay(c)) moduleAnimator.Play(c);
            }
        }

        public IEnumerable<IModuleAnimator> Animators =>
            _observer.ModuleToVisualObjects.Values.Select(x => (x as MonoBehaviour)?.GetComponent<IModuleAnimator>())
                .Where(x => x != null);

        private void OnAbilityUsed(IAbilityReadOnly ability)
        {
            foreach (IDataModule<IExtensibleItem> module in _observer.ModuleToVisualObjects.Keys)
            {
                if (module.Value.Extensions.TryGet(out IAbilityContainer ab) && ab.Ability == ability && ab.Ability is IFocusProvider focusProvider)
                {
                    if(_observer.ModuleToVisualObjects[module] is not MonoBehaviour mb || ! mb.TryGetComponent(out IModuleAnimator animator))continue;

                    animator.Play(new AbilityAnimationContext(focusProvider.GetFocusDirection(),focusProvider.FocusTime));
                }
            }
        }

        private void Move(Vector2 direction)
        {
            bool left = false;
            bool right = true;

            IDataModule<IExtensibleItem> from = _observer.ModuleToVisualObjects.Keys.FirstOrDefault(x => x.Value is ICoreExtensibleItem);

            if(from == null) return;
            
            foreach (IValueDataPort<PortDirection, IExtensibleItem> dataPort in from.DataPorts.OfType<IValueDataPort<PortDirection, IExtensibleItem>>())
            {
                if (dataPort.Value == PortDirection.Left)
                {
                    left = !left;
                }
                if (dataPort.Value == PortDirection.Left)
                {
                    right= !right;
                }
                
                if(!dataPort.IsConnected) continue;

                IDataModule<IExtensibleItem> module = dataPort.DataConnection.GetOtherPort(dataPort).DataModule;
                
                if (!_observer.ModuleToVisualObjects[module].ValueStorageContainer.TryGet(out IValueStorage<PortDirection> dir)) continue;
                if(_observer.ModuleToVisualObjects[module] is not MonoBehaviour mb || ! mb.TryGetComponent(out IModuleAnimator animator)) continue;
                
                switch (dir.Value)
                {
                    case PortDirection.Left:
                        animator.Play(new MoveAnimationContext(direction, left ? 0.5f : 0));
                        break;
                    case PortDirection.Right:
                        animator.Play(new MoveAnimationContext(direction, right ? 0.5f : 0));
                        break;
                }
            }
        }
    }
}