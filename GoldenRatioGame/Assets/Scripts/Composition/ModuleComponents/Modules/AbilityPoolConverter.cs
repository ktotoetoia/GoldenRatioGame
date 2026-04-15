using System;
using System.Linq;
using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolConverter : MonoBehaviour, IComponentConverter
    {
        public Type MutableType => typeof(AbilityPool);
        public Type ReadOnlyType => typeof(IAbilityPoolReadOnly);

        public object CreateReadOnly() => new AbilityPool();
        public object ToReadOnly(object mutable) => new AbilityPool((mutable as IAbilityPoolReadOnly).ToList());
        public object ToMutable(object readOnly) => new AbilityPool((readOnly as AbilityPool).ToList());
    }
}