using System;
using IM.SaveSystem;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class RandomSpriteSelectorSerializer : ComponentSerializer<RandomSpriteSelector>
    {
        public override object CaptureState(RandomSpriteSelector component)
        {
            return component.SavedSpriteIndex;
        }

        public override void RestoreState(RandomSpriteSelector component, object state, Func<string, GameObject> resolveDependency)
        {
            component.SavedSpriteIndex = (int)state;
        }
    }
}