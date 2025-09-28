using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Entities;
using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class BlinkAbilityModule : Module, IExtensibleModule, IRequireEntity
    {
        private readonly BlinkForwardAbility _blinkForwardAbility;
        private IEntity _entity;

        public IReadOnlyList<IModuleExtension> Extensions { get; }
        
        public IEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;

                if (value == null)
                {
                    _blinkForwardAbility.DirectionProvider = null;
                    _blinkForwardAbility.Rigidbody = null;
                    return;
                }
                
                _blinkForwardAbility.DirectionProvider = _entity.GameObject.GetComponent<IDirectionProvider>();
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