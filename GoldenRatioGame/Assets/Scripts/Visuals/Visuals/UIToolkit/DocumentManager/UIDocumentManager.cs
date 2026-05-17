using System.Collections.Generic;
using IM.Commands;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class UIDocumentManager : MonoBehaviour
    {
        [SerializeField] private List<DocumentContext> _documents;
        [SerializeField] private List<DocumentTransitionContext> _documentTransitions;
        private readonly CommandStack _stack = new();
        
        private void Awake()
        {
            Build();
        }

        private void Build()
        {
            foreach (DocumentContext context in _documents)
            {
                context.Document.rootVisualElement.visible = false;
                
                if (context.Document.rootVisualElement.Q<Button>(context.BackButtonName) is { } button)
                {
                    button.clicked += () => TryUndo(context);
                }
            }

            foreach (DocumentTransitionContext transition in _documentTransitions)
            {
                UIDocument from = _documents[transition.FromIndex].Document;
                UIDocument to = _documents[transition.ToIndex].Document;

                if (from.rootVisualElement.Q<Button>(transition.TransitionButtonName) is { } button)
                {
                    button.clicked += () =>
                    {
                        _stack.ExecuteAndPush(new UIDocumentTransitionCommand(new DocumentTransitionInfo(from,to,isOverlay:transition.IsOverlay)));
                    };
                }
            }

            foreach (DocumentContext context in _documents)
            {
                context.Document.rootVisualElement.visible = true;
                break;
            }
        }

        private void TryUndo(DocumentContext context)
        {
            _stack.UndoLast();
        }
    }
}