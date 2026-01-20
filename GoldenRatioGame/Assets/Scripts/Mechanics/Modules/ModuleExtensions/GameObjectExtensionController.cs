using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class GameObjectExtensionController : IExtensionController
    {
        private readonly GameObject _gameObject;
        private readonly List<IExtension> _extensions = new();

        public IReadOnlyList<IExtension> Extensions => _extensions;

        public GameObjectExtensionController(GameObject gameObject)
        {
            _gameObject = gameObject;
            
            gameObject.GetComponents(_extensions);
        }
        
        public void AddExtension(IExtension extension)
        {
            if (_extensions.Contains(extension)) throw new InvalidOperationException("Already contains this extension");
            if (extension is MonoBehaviour m && m.gameObject != _gameObject) throw new InvalidOperationException("Cannot add MonoBehaviour extension of a different gameObject") ;
            
            _extensions.Add(extension);
        }

        public void RemoveExtension(IExtension extension)
        {
            if(!_extensions.Contains(extension)) throw new Exception("Does not contain this extension");
            
            _extensions.Remove(extension);
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }

        public bool HasExtensionOfType<T>()
        {
            return Extensions.OfType<T>().Any();
        }

        public List<T> GetExtensions<T>()
        {
            return _extensions.OfType<T>().ToList();
        }
        
        public bool TryGetExtension<T>(out T extension)
        {
            extension = GetExtension<T>();
            return extension != null;
        }

        public bool TryGetExtension<T>(out List<T> extensions)
        {
            extensions = GetExtensions<T>();
            
            return extensions != null && extensions.Count != 0;
        }
    }
}