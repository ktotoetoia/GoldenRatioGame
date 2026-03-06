using System;
using UnityEngine;

namespace IM.Items
{
    [Serializable]
    public class Icon : IIcon
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        public Icon(Sprite texture)
        {
            Sprite = texture;
        }
    }
}