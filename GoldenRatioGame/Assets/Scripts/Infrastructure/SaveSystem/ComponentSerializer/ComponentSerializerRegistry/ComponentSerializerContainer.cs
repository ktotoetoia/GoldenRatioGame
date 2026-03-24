using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IM.SaveSystem
{
    public class ComponentSerializerContainer : IComponentSerializerContainer
    {
        private readonly Dictionary<Type, IComponentSerializer> _map = new();

        public ComponentSerializerContainer(IEnumerable<IComponentSerializer> manualSerializers = null)
        {
            var passedTypes = new HashSet<Type>();

            if (manualSerializers != null)
            {
                foreach (var serializer in manualSerializers)
                {
                    _map[serializer.TargetType] = serializer;
                    passedTypes.Add(serializer.GetType());
                }
            }

            RegisterAll(passedTypes);
        }

        private void RegisterAll(HashSet<Type> ignoredTypes)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var asm in assemblies)
            {
                if (asm.IsDynamic || asm.FullName.StartsWith("System") || asm.FullName.StartsWith("Microsoft")) 
                    continue;

                Type[] types;
                try { types = asm.GetTypes(); } 
                catch { continue; }

                foreach (var t in types)
                {
                    if (typeof(IComponentSerializer).IsAssignableFrom(t) && 
                        !t.IsInterface && 
                        !t.IsAbstract && 
                        !ignoredTypes.Contains(t))
                    {
                        var constructor = t.GetConstructor(
                            BindingFlags.Instance | BindingFlags.Public, 
                            null, 
                            Type.EmptyTypes, 
                            null);

                        if (constructor != null)
                        {
                            if (Activator.CreateInstance(t) is IComponentSerializer inst)
                            {
                                _map.TryAdd(inst.TargetType, inst);
                            }
                        }
                    }
                }
            }
        }

        public IComponentSerializer GetSerializerFor(Type componentType)
        {
            if (_map.TryGetValue(componentType, out var s)) return s;

            return _map.FirstOrDefault(kv => kv.Key.IsAssignableFrom(componentType)).Value;
        }
    }
}