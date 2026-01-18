using System;
using UnityEngine;

namespace IM.Items
{
    [Serializable]
    public class Icon : IIcon
    {
        [field: SerializeField] public Sprite Texture { get; private set; }
        
        public Icon(Sprite texture)
        {
            Texture = texture;
        }
    }
}