using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public static class TransformOperations
    {
        public static (Vector3 pos, Vector3 scale, Quaternion rot) ComputeWorld(
            ITransformReadOnly parent,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation)
        {
            if (parent == null)
                return (localPosition, localScale, localRotation);

            Vector3 worldPos = parent.Position + parent.Rotation * Vector3.Scale(localPosition, parent.LossyScale);
            Vector3 worldScale = Vector3.Scale(parent.LossyScale, localScale);
            Quaternion worldRot = parent.Rotation * localRotation;
            return (worldPos, worldScale, worldRot);
        }

        /// <summary>
        /// Compute world transforms for multiple local transforms given a single parent.
        /// Returns an array of tuples in the same order as the inputs.
        /// </summary>
        public static (Vector3 pos, Vector3 scale, Quaternion rot)[] ComputeWorlds(
            ITransformReadOnly parent,
            IEnumerable<ITransformReadOnly> locals)
        {
            var list = new List<(Vector3, Vector3, Quaternion)>();
            if (locals == null) return list.ToArray();

            foreach (var l in locals)
            {
                list.Add(ComputeWorld(parent, l.LocalPosition, l.LocalScale, l.LocalRotation));
            }

            return list.ToArray();
        }

        /// <summary>
        /// Given a desired world transform and an optional parent, computes & sets the local transform on target.
        /// Uses safe divide for scale components (if parent scale component is zero, leaves local scale same as world scale).
        /// </summary>
        public static void SetLocalFromWorld(
            ITransform target,
            Vector3 worldPosition,
            Vector3 worldScale,
            Quaternion worldRotation,
            ITransformReadOnly parent)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (parent == null)
            {
                target.LocalPosition = worldPosition;
                target.LocalScale = worldScale;
                target.LocalRotation = worldRotation;
            }
            else
            {
                Quaternion localRot = Quaternion.Inverse(parent.Rotation) * worldRotation;

                Vector3 delta = worldPosition - parent.Position;
                Vector3 unrotated = Quaternion.Inverse(parent.Rotation) * delta;

                Vector3 invParentScale = SafeInverse(parent.LossyScale);
                Vector3 localPos = Vector3.Scale(unrotated, invParentScale);

                Vector3 localScale = new Vector3(
                    parent.LossyScale.x != 0f ? worldScale.x / parent.LossyScale.x : worldScale.x,
                    parent.LossyScale.y != 0f ? worldScale.y / parent.LossyScale.y : worldScale.y,
                    parent.LossyScale.z != 0f ? worldScale.z / parent.LossyScale.z : worldScale.z
                );

                target.LocalPosition = localPos;
                target.LocalScale = localScale;
                target.LocalRotation = localRot;
            }
        }

        /// <summary>
        /// Batch SetLocalFromWorld for multiple targets using the same parent.
        /// </summary>
        public static void SetLocalFromWorlds(
            IEnumerable<ITransform> targets,
            IEnumerable<(Vector3 pos, Vector3 scale, Quaternion rot)> worlds,
            ITransformReadOnly parent)
        {
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            if (worlds == null) throw new ArgumentNullException(nameof(worlds));

            using (var tEnum = targets.GetEnumerator())
            using (var wEnum = worlds.GetEnumerator())
            {
                while (tEnum.MoveNext() && wEnum.MoveNext())
                {
                    SetLocalFromWorld(tEnum.Current, wEnum.Current.pos, wEnum.Current.scale, wEnum.Current.rot, parent);
                }
            }
        }

        /// <summary>
        /// Compute local rotation that would produce the given world rotation under parent.
        /// Returns worldRotation unchanged if parent == null.
        /// </summary>
        public static Quaternion LocalRotationFromWorld(Quaternion worldRotation, ITransformReadOnly parent)
        {
            if (parent == null) return worldRotation;
            return Quaternion.Inverse(parent.Rotation) * worldRotation;
        }

        private static Vector3 SafeInverse(Vector3 v)
        {
            return new Vector3(
                v.x != 0f ? 1f / v.x : 1f,
                v.y != 0f ? 1f / v.y : 1f,
                v.z != 0f ? 1f / v.z : 1f
            );
        }
    }
}