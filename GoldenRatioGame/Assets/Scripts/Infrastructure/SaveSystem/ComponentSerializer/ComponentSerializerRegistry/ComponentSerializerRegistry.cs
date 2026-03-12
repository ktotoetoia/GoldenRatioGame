using System;
using System.Collections.Generic;

namespace IM.SaveSystem
{
    public class ComponentSerializerRegistry : IComponentSerializerRegistry
    {

        private readonly Dictionary<Type, IComponentSerializer> _map = new();

        public ComponentSerializerRegistry()
        {
            RegisterAll();
        }

        private void RegisterAll()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                Type[] types;
                try { types = asm.GetTypes(); } 
                catch { continue; }

                foreach (var t in types)
                {
                    if (typeof(IComponentSerializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                    {
                        if (Activator.CreateInstance(t) is IComponentSerializer inst)
                        {
                            _map[inst.TargetType] = inst;
                        }
                    }
                }
            }
        }

        public IComponentSerializer GetSerializerFor(Type componentType)
        {
            if (_map.TryGetValue(componentType, out var s)) return s;

            foreach (var kv in _map)
            {
                if (kv.Key.IsAssignableFrom(componentType))
                    return kv.Value;
            }

            return null;
        }
    }
}