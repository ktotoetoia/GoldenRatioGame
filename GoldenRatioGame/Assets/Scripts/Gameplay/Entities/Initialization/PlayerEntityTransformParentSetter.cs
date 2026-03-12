using UnityEngine;

namespace IM.Entities
{
    public class PlayerEntityTransformParentSetter : MonoBehaviour, IRequirePlayerEntity
    {
        public void SetPlayerEntity(IEntity playerEntity)
        {
            playerEntity.GameObject.transform.SetParent(transform);
        }
    }
}