using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public sealed class StatPreviewContainer : VisualElement
    {
        private const string RootClass = "stat-preview-container";
        private const string PreviewClass = "stat-preview-container__preview";
        private const string SpacedPreviewClass = "stat-preview-container__preview--spaced";

        private readonly IReadOnlyList<IStatPreviewer> _previewers;
        private readonly List<PreviewBinding> _bindings = new();

        public bool HasContent => _bindings.Count > 0;

        public StatPreviewContainer(IEnumerable<IStatPreviewer> previewers)
        {
            _previewers = previewers
                .Where(previewer => previewer != null)
                .ToArray();

            AddToClassList(RootClass);
        }

        public void Bind(object item)
        {
            Unbind();

            foreach (IStatPreviewer previewer in _previewers)
            {
                VisualElement preview = previewer.GetPreview(item);

                if (preview == null) continue;

                preview.AddToClassList(PreviewClass);
                preview.EnableInClassList(SpacedPreviewClass, childCount > 0);

                Add(preview);

                previewer.UpdatePreview(preview, item);
                _bindings.Add(new PreviewBinding(previewer, preview));
            }
        }

        public void UpdatePreviews(object item)
        {
            foreach (PreviewBinding binding in _bindings)
                binding.Previewer.UpdatePreview(binding.Element, item);
        }

        public void Unbind()
        {
            _bindings.Clear();
            Clear();
        }

        private readonly struct PreviewBinding
        {
            public IStatPreviewer Previewer { get; }
            public VisualElement Element { get; }

            public PreviewBinding(IStatPreviewer previewer, VisualElement element)
            {
                Previewer = previewer;
                Element = element;
            }
        }
    }
}