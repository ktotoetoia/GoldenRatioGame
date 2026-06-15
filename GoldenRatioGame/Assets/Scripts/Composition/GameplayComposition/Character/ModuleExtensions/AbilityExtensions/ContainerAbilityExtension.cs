using System.Linq;
using IM.Abilities;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class ContainerAbilityExtension : MonoBehaviour, IAbilityExtension, IRequireEntityExtension
    {
        private IAbilityContainer _abilityContainer;

        private IAbilityContainer AbilityContainer => _abilityContainer??=GetComponents<IAbilityContainer>().FirstOrDefault(x => !ReferenceEquals(x, this));
        
        public IAbilityReadOnly Ability => AbilityContainer.Ability;
        public IEntity Entity { get; set; }
    }
}