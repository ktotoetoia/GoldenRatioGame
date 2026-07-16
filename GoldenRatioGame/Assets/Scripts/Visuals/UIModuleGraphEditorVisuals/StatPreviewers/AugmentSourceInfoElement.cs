using System.Collections.Generic;
using System.Linq;
using IM.Augments;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    [UxmlElement]
    public partial class AugmentSourceInfoElement : VisualElement, ITooltipInfo
    {
        private const string RootClass  = "augment-source-info";
        private const string EmptyClass = "augment-source-info--empty";

        private readonly AugmentPreviewElement _preview;
        private IAugmentProgress _progress;
        private AugmentInfo? _currentAugmentInfo;
        
        public bool IsFinished { get; private set; }

        public AugmentSourceInfoElement()
        {
            AddToClassList(RootClass);

            _preview = new AugmentPreviewElement();
            _preview.TooltipDisabled = true;
            Add(_preview);
        }

        public void SetSource(IAugmentProgress progress)
        {
            _progress = progress;

            List<AugmentInfo> augments = progress.Augments.ToList();
            bool hasAny = augments.Count > 0;

            EnableInClassList(EmptyClass, !hasAny);
            _preview.style.display = hasAny ? DisplayStyle.Flex : DisplayStyle.None;

            if (!hasAny)
            {
                _currentAugmentInfo = null;
                IsFinished = false;
                _preview.Clear();
                return;
            }

            bool allFinished = progress.Progress.CurrentIndex >= augments.Count;
            int displayIndex = allFinished ? augments.Count - 1 : progress.Progress.CurrentIndex;

            AugmentInfo augmentInfo = augments[displayIndex];
            _currentAugmentInfo = augmentInfo;
            IsFinished = allFinished;

            IAugment augment = augmentInfo.Factory.Create(new AugmentContext(null));

            float currentProgress = allFinished ? augmentInfo.RequiredProgress : progress.Progress.Value;

            _preview.SetAugment(augmentInfo, augment, currentProgress, progress.GetCurrentAugments().Any(x => x.Factory == augmentInfo.Factory));

            schedule.Execute(MarkDirtyRepaint);
        }

        public string Name => "Augments";
        public string ShortDescription => null;
        public string Description => null;
        public Sprite Icon => _preview.Augment?.Icon?.Sprite;
        public object Item => _progress;
        public bool TooltipDisabled { get; set; }
    }
}