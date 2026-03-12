using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public interface IKeyAbilityPoolReadOnly : IAbilityPoolReadOnly
    {
        public IReadOnlyDictionary<KeyCode, IAbilityReadOnly> KeyMap { get; }
    }
}