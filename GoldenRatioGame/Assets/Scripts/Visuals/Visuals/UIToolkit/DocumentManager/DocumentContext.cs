using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [Serializable]
    public class DocumentContext
    {
        [SerializeField] private string _backButtonName = "BackButton";
        [SerializeField] private UIDocument _document;
        
        public UIDocument Document => _document;
        public string BackButtonName => _backButtonName;
    }
}