using System;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class SerializedValue
    {
        public UnityEngine.Object objectValue;
        public int intValue;
        public float floatValue;
        public double doubleValue;
        public bool boolValue;
        public string stringValue;
        public int enumIndex;

        public object GetValue(Type targetType)
        {
            if (targetType == typeof(string))
                return stringValue;

            if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
                return objectValue != null && targetType.IsAssignableFrom(objectValue.GetType())
                    ? objectValue
                    : null;

            if (targetType.IsEnum)
                return Enum.ToObject(targetType, enumIndex);

            try
            {
                if (targetType == typeof(bool))
                    return boolValue;

                if (targetType == typeof(float))
                    return floatValue;

                if (targetType == typeof(double))
                    return doubleValue;

                if (IsInteger(targetType))
                    return Convert.ChangeType(intValue, targetType);

                if (!string.IsNullOrEmpty(stringValue))
                    return Convert.ChangeType(stringValue, targetType);

                return Convert.ChangeType(intValue, targetType);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"SerializedValue conversion failed → {targetType.Name}: {e.Message}");
                return null;
            }
        }

        private static bool IsInteger(Type t)
        {
            return t == typeof(int) || t == typeof(short) || t == typeof(byte) ||
                   t == typeof(long) || t == typeof(uint) || t == typeof(ulong) ||
                   t == typeof(ushort) || t == typeof(sbyte);
        }
    }
}