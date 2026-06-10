using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public static class WorldDocumentUtility
    {
        public static IEnumerable<VisualElement> GetElementsAtPosition(UIDocument document, VisualElement from, Vector3 position)
        {
            foreach (VisualElement element in from.Query<VisualElement>().ToList())
            {
                var bounds = element.worldBound;
                bounds.position += (Vector2)document.transform.position;
            
                if (bounds.Contains(position)) yield return element;
            }
        }

        public static IEnumerable<VisualElement> GetElementsAtPosition(UIDocument document, Vector3 position)
        {
            return GetElementsAtPosition(document, document.rootVisualElement, position);
        }

        public static IEnumerable<T> GetElementsAtPosition<T>(UIDocument document, VisualElement from, Vector3 position)
        {
            return GetElementsAtPosition(document, from, position).OfType<T>();
        }

        public static IEnumerable<T> GetElementsAtPosition<T>(UIDocument document, Vector3 position)
        {
            return GetElementsAtPosition(document, document.rootVisualElement, position).OfType<T>();
        }
    }
}