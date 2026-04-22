using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class TypedStorageElementTypeSetter : MonoBehaviour
    {
        [SerializeField] private string _typeName;
        private UIDocument _document;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            
            Type type = FindTypeByName(_typeName);
            TypedStorageElement storage = _document.rootVisualElement.Query().ToList().OfType<TypedStorageElement>().FirstOrDefault();
            
            if(storage == null || type == null) return;
            
            storage.TargetType = type;
        }
        
        private static Type FindTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            Type type = Type.GetType(typeName);
            if (type != null) return type;

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);
        }
    }
}