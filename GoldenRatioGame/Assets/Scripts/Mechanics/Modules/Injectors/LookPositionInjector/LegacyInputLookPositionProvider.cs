using UnityEngine;

namespace IM.Modules
{
    public class LegacyInputLookPositionProvider : ILookPositionProvider
    {
        public Vector2 GetLookPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}