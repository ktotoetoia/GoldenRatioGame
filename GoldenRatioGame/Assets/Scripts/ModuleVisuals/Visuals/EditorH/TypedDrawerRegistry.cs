using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Visuals
{
    public class TypedDrawerRegistry
    {
        private readonly List<(Func<Type,bool> Match, ITypedDrawer Drawer)> _entries = new();

        public TypedDrawerRegistry()
        {
            _entries.Add((t => typeof(UnityEngine.Object).IsAssignableFrom(t), new ObjectDrawer()));
            _entries.Add((t => t.IsEnum, new EnumDrawer()));
            _entries.Add((t => t == typeof(int), new IntDrawer()));
            _entries.Add((t => t == typeof(float), new FloatDrawer()));
            _entries.Add((t => t == typeof(bool), new BoolDrawer()));
            _entries.Add((t => t == typeof(string), new StringDrawer()));
            _entries.Add((t => true, new DefaultDrawer()));
        }

        public ITypedDrawer Get(Type t) => _entries.First(e => e.Match(t)).Drawer;
    }
}