using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Entities;
using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class BlinkAbilityModuleContext : IModuleContext, IRequireEntity
    {
        private readonly BlinkForwardAbility _blinkForwardAbility;
        private IEntity _entity;
        public IModuleContextExtensions Extensions { get; }
        
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

        public BlinkAbilityModuleContext()
        {
            Extensions = new ModuleContextExtensions(new IModuleExtension[]
            {
                new AbilityExtension(_blinkForwardAbility = new BlinkForwardAbility(new FloatCooldown(1))),
                new HealthExtension(0,100,100)
            });
        }

        public IModule Create()
        {
            ModuleContextWrapper module =  new ModuleContextWrapper(this);
            module.AddPort(new ModulePort(module,PortDirection.Output));
            module.AddPort(new ModulePort(module,PortDirection.Input));

            return module;
        }
    }
}