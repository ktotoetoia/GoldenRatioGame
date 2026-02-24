using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class DebugDrawLineAbilityExtensions : MonoBehaviour, IAbilityExtension
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private float _channelTime;
        private DebugDrawLineChannelAbility _debugDrawLine;
        
        public IAbilityReadOnly Ability => _debugDrawLine;

        private void Awake()
        {
            _debugDrawLine = new DebugDrawLineChannelAbility(_cooldown,_channelTime);
        }

        private void Update()
        {
            _debugDrawLine.Update();
        }
    }
}