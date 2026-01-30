using System;
using UnityEngine;

namespace IM.Transforms
{
    public static class TransformOperations
    {
        private static Matrix4x4 TRS(Vector3 pos, Quaternion rot, Vector3 scale) =>
            Matrix4x4.TRS(pos, rot, scale);

        private static void DecomposeMatrix(Matrix4x4 m, out Vector3 pos, out Vector3 scale, out Quaternion rot)
        {
            pos = m.GetColumn(3);

            Vector3 col0 = new Vector3(m.m00, m.m10, m.m20);
            Vector3 col1 = new Vector3(m.m01, m.m11, m.m21);
            Vector3 col2 = new Vector3(m.m02, m.m12, m.m22);

            float sx = col0.magnitude;
            float sy = col1.magnitude;
            float sz = col2.magnitude;

            Vector3 cross = Vector3.Cross(col0, col1);
            float detSign = (Vector3.Dot(cross, col2) < 0f) ? -1f : 1f;
            if (detSign < 0f)
            {
                sx = -sx;
                col0 = -col0;
            }

            Vector3 norm0 = (sx == 0f) ? Vector3.right : col0 / Mathf.Abs(sx);
            Vector3 norm1 = (sy == 0f) ? Vector3.up    : col1 / Mathf.Abs(sy);
            Vector3 norm2 = (sz == 0f) ? Vector3.forward: col2 / Mathf.Abs(sz);

            rot = Quaternion.LookRotation(norm2, norm1);

            scale = new Vector3(sx, sy, sz);
        }

        public static (Vector3 pos, Vector3 scale, Quaternion rot) ComputeWorld(
            ITransformReadOnly parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            if (parent == null)
                return (localPosition, localScale, localRotation);

            var parentMatrix = TRS(parent.Position, parent.Rotation, parent.LossyScale);
            var localMatrix = TRS(localPosition, localRotation, localScale);
            var worldMatrix = parentMatrix * localMatrix;

            DecomposeMatrix(worldMatrix, out Vector3 worldPos, out Vector3 worldScale, out Quaternion worldRot);
            return (worldPos, worldScale, worldRot);
        }

        public static void SetLocalFromWorld(
            ITransform target, Vector3 worldPosition, Vector3 worldScale, Quaternion worldRotation, ITransformReadOnly parent)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (parent == null)
            {
                target.LocalPosition = worldPosition;
                target.LocalScale = worldScale;
                target.LocalRotation = worldRotation;
                return;
            }

            var worldMatrix = TRS(worldPosition, worldRotation, worldScale);

            var parentMatrix = TRS(parent.Position, parent.Rotation, parent.LossyScale);

            var invParent = parentMatrix.inverse;
            var localMatrix = invParent * worldMatrix;

            DecomposeMatrix(localMatrix, out Vector3 localPos, out Vector3 localScale, out Quaternion localRot);

            target.LocalPosition = localPos;
            target.LocalScale = localScale;
            target.LocalRotation = localRot;
        }

        public static Quaternion LocalRotationFromWorld(Quaternion worldRotation, ITransformReadOnly parent)
        {
            if (parent == null) return worldRotation;

            var worldMatrix = TRS(Vector3.zero, worldRotation, Vector3.one);
            var parentMatrix = TRS(Vector3.zero, parent.Rotation, parent.LossyScale);

            var invParent = parentMatrix.inverse;
            var localMatrix = invParent * worldMatrix;

            DecomposeMatrix(localMatrix, out _, out Vector3 localScale, out Quaternion localRot);
            return localRot;
        }
    }

}