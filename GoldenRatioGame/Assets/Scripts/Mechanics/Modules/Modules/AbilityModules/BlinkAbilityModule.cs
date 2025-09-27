using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Entities;
using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class BlinkAbilityModule : Module, IExtensibleModule, IRequireEntity, IRequireLookPosition
    {
        private readonly BlinkForwardAbility _blinkForwardAbility;
        private IEntity _entity;
        private ILookPositionProvider _lookPositionProvider;

        public IReadOnlyList<IModuleExtension> Extensions { get; }

        public ILookPositionProvider LookPositionProvider
        {
            get => _lookPositionProvider;
            set
            {
                _lookPositionProvider = value;
                _blinkForwardAbility.GetLookDirection = _lookPositionProvider.GetLookPosition;
            }
        }

        public IEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                _blinkForwardAbility.Rigidbody = _entity.GameObject.GetComponent<Rigidbody2D>();
            }
        }

        public BlinkAbilityModule()
        {
            Extensions = new List<IModuleExtension>()
            {
                new AbilityExtension(_blinkForwardAbility = new BlinkForwardAbility(new FloatCooldown(1))),
            };
            
            _ports.Add(new ModulePort(this,PortDirection.Input));
            _ports.Add(new ModulePort(this, PortDirection.Output));
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}