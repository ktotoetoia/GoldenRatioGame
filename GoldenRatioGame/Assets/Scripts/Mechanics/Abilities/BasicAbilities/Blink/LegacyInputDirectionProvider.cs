using UnityEngine;

namespace IM.Abilities
{
    public class LegacyInputDirectionProvider : MonoBehaviour, IDirectionProvider
    {
        public Vector2 GetDirection()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        }
    }
}