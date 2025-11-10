using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleExtensions : IModuleExtensions
    {
        private readonly GameObject _gameObject;
        private readonly List<IModuleExtension> _extensions = new();

        public IReadOnlyList<IModuleExtension> Extensions => _extensions;

        public ModuleExtensions(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public void AddExtension(IModuleExtension extension)
        {
            if (_extensions.Contains(extension)) throw new InvalidOperationException("Already contains this extension");
            if (extension is MonoBehaviour m && m.gameObject != _gameObject) throw new InvalidOperationException("Cannot add MonoBehaviour extension of a different gameObject") ;
            
            _extensions.Add(extension);
        }

        public void RemoveExtension(IModuleExtension extension)
        {
            if(!_extensions.Contains(extension)) throw new Exception("Does not contain this extension");
            if (extension is MonoBehaviour m) throw new InvalidOperationException("Cannot remove MonoBehaviour extension");
            
            _extensions.Remove(extension);
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}