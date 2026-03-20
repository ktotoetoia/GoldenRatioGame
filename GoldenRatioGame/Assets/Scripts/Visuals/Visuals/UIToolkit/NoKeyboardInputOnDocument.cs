using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class NoKeyboardInputOnDocument : MonoBehaviour
    {
        private void Start()
        {
            UIDocument uiDoc = GetComponent<UIDocument>();
            VisualElement root = uiDoc.rootVisualElement;

            root.focusable = false;
            
            foreach (VisualElement element in root.Query<VisualElement>().ToList())
            {
                element.focusable = false;
            }
        }
    }
}