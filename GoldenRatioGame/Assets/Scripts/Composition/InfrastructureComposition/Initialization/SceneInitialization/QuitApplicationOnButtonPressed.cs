using UnityEngine;
using UnityEngine.UIElements;

namespace Tests
{
    public class QuitApplicationOnButtonPressed : MonoBehaviour
    {
        [SerializeField] private string _quitButton = "QuitApplicationButton";
        private UIDocument _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _document.rootVisualElement.Q<Button>(_quitButton).clicked += Quit;
        }

        private static void Quit() => Application.Quit();
    }
}