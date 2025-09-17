using UnityEngine;

namespace IM.Abilities
{
    public interface IPreferredKeyboardBinding
    {
        KeyCode Key { get; set; }
    }
}