using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public class ClickThroughDocument : MonoBehaviour
    {
        void Start()
        {
            UIDocument uiDoc = GetComponent<UIDocument>();
            VisualElement root = uiDoc.rootVisualElement;

            root.pickingMode = PickingMode.Ignore;

            foreach (VisualElement element in root.Query<VisualElement>().ToList())
            {
                element.pickingMode = PickingMode.Ignore;
            }
        }
    }
}