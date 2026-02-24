using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public interface IKeyAbilityPool : IAbilityPoolReadOnly
    {
        public IReadOnlyDictionary<KeyCode, IAbilityReadOnly> KeyMap { get; }
    }
}