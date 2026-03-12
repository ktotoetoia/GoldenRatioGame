using System;
using System.Collections.Generic;

namespace IM.SaveSystem
{
    /// <summary>
    /// Prevents save corruption when namespaces or class names change.
    /// </summary>
    public static class TypeCache
    {
        private static readonly Dictionary<string, Type> _aliasToType = new();

        public static string GetAlias(Type type) => type.AssemblyQualifiedName;

        public static Type GetType(string alias)
        {
            if (_aliasToType.TryGetValue(alias, out var type)) return type;
            
            type = Type.GetType(alias);
            if (type != null) _aliasToType[alias] = type;
            
            return type;
        }
    }
}