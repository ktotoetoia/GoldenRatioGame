using System;
using UnityEngine;

namespace IM.Health
{
    public readonly struct HealthChangeResult : IEquatable<HealthChangeResult>
    {
        public float PreMitigation { get; }
        public float PostMitigationUnclamped { get; }
        public float Applied { get; }

        public float Mitigated => PreMitigation - PostMitigationUnclamped;
        public float Overflow => Mathf.Max(0f, PostMitigationUnclamped - Applied);
    
        public HealthChangeResult(float preMitigation, float postUnclamped, float applied)
        {
            PreMitigation = preMitigation;
            PostMitigationUnclamped = postUnclamped;
            Applied = applied;
        }

        public bool Equals(HealthChangeResult other)
        {
            return PreMitigation.Equals(other.PreMitigation) && PostMitigationUnclamped.Equals(other.PostMitigationUnclamped) && Applied.Equals(other.Applied);
        }

        public override bool Equals(object obj)
        {
            return obj is HealthChangeResult other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PreMitigation, PostMitigationUnclamped, Applied);
        }

        public override string ToString()
        {
            return $"PreMitigation: {PreMitigation}, PostMitigationUnclamped: {PostMitigationUnclamped}, Applied: {Applied}";
        }
    }
}