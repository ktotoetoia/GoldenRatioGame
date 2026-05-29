using IM.Factions;
using IM.Map;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class SeekTargetInRoomEntityAction : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IFactionMemberReadOnly _owner;
        private readonly IRoomVisitor _ownerRoomVisitor;
        private readonly TargetMemory _targetMemory; 

        private const int EnvironmentLayer = 9;
        private const int EnvironmentMask = 1 << EnvironmentLayer;
        
        public float DetectionRange { get; set; } = 10f;

        public SeekTargetInRoomEntityAction(Transform ownerTransform, IMemoryContainer ownerMemory,
            IFactionMemberReadOnly owner, IRoomVisitor ownerRoomVisitor)
        {
            _ownerTransform = ownerTransform;
            _owner = owner;
            _ownerRoomVisitor = ownerRoomVisitor;
            if (ownerMemory.Memories.TryGet(out _targetMemory)) return;
            
            _targetMemory = new TargetMemory();
            ownerMemory.Add(_targetMemory);
        }

        public override void Update()
        {
            IGameObjectRoom room = _ownerRoomVisitor.CurrentRoom;
            if (room == null) return;
            
            Vector3 ownerPos = _ownerTransform.position;
            float sqrDetectionRange = DetectionRange * DetectionRange; 
            GameObject closest = null;
            Vector3 closestPos = Vector3.zero;
            float closestSqrDistance = float.MaxValue;

            foreach (GameObject candidate in room.GameObjects)
            {
                if (!candidate) continue;

                Vector3 candidatePos = candidate.transform.position;
                Vector3 offset = candidatePos - ownerPos;
                float sqrDistance = offset.sqrMagnitude;

                if (sqrDistance > sqrDetectionRange) continue;
                if (sqrDistance >= closestSqrDistance) continue;
                if (!candidate.TryGetComponent<IFactionMemberReadOnly>(out var factionMember)) continue;
                if (_owner.Faction.GetRelationWith(factionMember.Faction) != FactionRelation.Enemy) continue;

                float distance = Mathf.Sqrt(sqrDistance); 
                if (!HasLineOfSight(ownerPos, offset, distance)) continue;

                closestSqrDistance = sqrDistance;
                closest = candidate;
                closestPos = candidatePos;
            }

            if (!closest)
            {
                _targetMemory.IsSeen = false;
                return;
            }
            _targetMemory.Target = closest;
            _targetMemory.IsSeen = true;
            _targetMemory.LastSeenAt = closestPos;
        }

        private bool HasLineOfSight(Vector2 origin, Vector2 offset, float distance)
        {
            Vector2 direction = offset / distance;
            return !Physics2D.Raycast(origin, direction, distance, EnvironmentMask);
        }
    }
}