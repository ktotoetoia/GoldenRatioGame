using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class AugmentProgressRowElement : VisualElement
    {
        private const string RootClass    = "augment-progress-row";
        private const string EmptyClass   = "augment-progress-row--empty";
        private const string RowItemClass = "augment-preview--row-item";

        private readonly List<AugmentPreviewElement> _previewElements = new();

        public IAugmentProgress Progress { get; private set; }

        public AugmentProgressRowElement()
        {
            AddToClassList(RootClass);
        }

        public void SetProgress(IAugmentProgress progress)
        {
            Progress = progress;

            List<AugmentInfo> augments = progress.Augments.ToList();
            bool hasAny = augments.Count > 0;

            EnableInClassList(EmptyClass, !hasAny);

            if (!hasAny)
            {
                ClearAll();
                return;
            }

            EnsureElementCount(augments.Count);

            AugmentProgressInfo progressInfo = progress.Progress;

            for (int i = 0; i < augments.Count; i++)
            {
                AugmentInfo augmentInfo = augments[i];
                AugmentPreviewElement preview = _previewElements[i];
                preview.ShowShortDescription = true;
                float progressValue = GetProgressForIndex(i, progressInfo, augmentInfo);
                IAugment augment = augmentInfo.Factory.Create(new AugmentContext(null));

                preview.SetAugment(augmentInfo, augment, progressValue, progress.GetCurrentAugments().Any(x => x.Factory == augmentInfo.Factory));
            }
        }

        private static float GetProgressForIndex(int index, AugmentProgressInfo progress, AugmentInfo augmentInfo)
        {
            if (index < progress.CurrentIndex) return augmentInfo.RequiredProgress;
            if (index > progress.CurrentIndex) return 0f;
            return progress.Value;
        }

        private void EnsureElementCount(int count)
        {
            while (_previewElements.Count < count)
            {
                var preview = new AugmentPreviewElement();
                preview.AddToClassList(RowItemClass);
                Add(preview);
                _previewElements.Add(preview);
            }

            while (_previewElements.Count > count)
            {
                int lastIndex = _previewElements.Count - 1;
                Remove(_previewElements[lastIndex]);
                _previewElements.RemoveAt(lastIndex);
            }
        }

        private void ClearAll()
        {
            foreach (AugmentPreviewElement preview in _previewElements) Remove(preview);
            _previewElements.Clear();
        }
    }
}