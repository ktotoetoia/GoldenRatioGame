using UnityEngine;
using UnityEngine.UIElements;

public sealed class PopupElement : VisualElement
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }
    
    public enum PreferredSide
    {
        Auto,
        Forward,
        Backward
    }

    private readonly Orientation _orientation;
    private PreferredSide _side;
    private float _crossAxisAlignment = 0.5f;
    private Vector2 _anchor;
    private bool _isVisible;
    private bool _layoutPending;
    
    public float AnchorGap { get; set; } = 6f;

    public PreferredSide Side
    {
        get => _side;
        set { _side = value; if (_isVisible) TryUpdatePosition(); }
    }
    
    public float CrossAxisAlignment
    {
        get => _crossAxisAlignment;
        set
        {
            _crossAxisAlignment = Mathf.Clamp01(value);
            if (_isVisible) TryUpdatePosition();
        }
    }

    public bool IsVisible => _isVisible;

    public PopupElement(
        Orientation orientation = Orientation.Vertical,
        PreferredSide preferredSide = PreferredSide.Auto)
    {
        _orientation = orientation;
        _side = preferredSide;

        style.position = Position.Absolute;
        style.display = DisplayStyle.None;
        style.flexGrow = 0;
        style.flexShrink = 1;
        style.alignSelf = Align.FlexStart;

        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    public void Show(Vector2 anchor, VisualElement content)
    {
        _anchor = anchor;
        _isVisible = true;
        _layoutPending = true;

        Clear();
        Add(content);
        style.display = DisplayStyle.Flex;

        TryUpdatePosition();
    }

    public void SetAnchor(Vector2 anchor)
    {
        _anchor = anchor;
        if (_isVisible) TryUpdatePosition();
    }

    public void Hide()
    {
        _isVisible = false;
        _layoutPending = false;
        style.display = DisplayStyle.None;
        Clear();
    }

    private void OnGeometryChanged(GeometryChangedEvent _)
    {
        if (_layoutPending) TryUpdatePosition();
    }

    private void TryUpdatePosition()
    {
        var parent = hierarchy.parent;
        if (parent == null) return;

        float w = resolvedStyle.width;
        float h = resolvedStyle.height;

        if (float.IsNaN(w) || float.IsNaN(h) || w <= 0f || h <= 0f) return;

        _layoutPending = false;

        Rect bounds = parent.contentRect;

        var (x, y) = _orientation == Orientation.Horizontal
            ? PlaceHorizontal(w, h, bounds)
            : PlaceVertical(w, h, bounds);

        style.left = x;
        style.top = y;
    }

    private (float x, float y) PlaceHorizontal(float w, float h, Rect bounds)
    {
        float forwardX  = _anchor.x + AnchorGap;
        float backwardX = _anchor.x - w - AnchorGap;

        float x = PickSide(
            fitsForward:   forwardX + w <= bounds.xMax,
            fitsBackward:  backwardX   >= bounds.xMin,
            forwardPos:    forwardX,
            backwardPos:   backwardX,
            spaceForward:  bounds.xMax - _anchor.x,
            spaceBackward: _anchor.x   - bounds.xMin);

        x = ClampSafe(x, bounds.xMin, bounds.xMax - w);

        float y = ClampSafe(_anchor.y - h * (1f - _crossAxisAlignment), bounds.yMin, bounds.yMax - h);

        return (x, y);
    }

    private (float x, float y) PlaceVertical(float w, float h, Rect bounds)
    {
        float forwardY  = _anchor.y + AnchorGap;
        float backwardY = _anchor.y - h - AnchorGap;

        float y = PickSide(
            fitsForward:   forwardY + h <= bounds.yMax,
            fitsBackward:  backwardY   >= bounds.yMin,
            forwardPos:    forwardY,
            backwardPos:   backwardY,
            spaceForward:  bounds.yMax - _anchor.y,
            spaceBackward: _anchor.y   - bounds.yMin);

        y = ClampSafe(y, bounds.yMin, bounds.yMax - h);

        float x = ClampSafe(_anchor.x - w * (1f - _crossAxisAlignment), bounds.xMin, bounds.xMax - w);

        return (x, y);
    }
    
    private float PickSide(
        bool fitsForward, bool fitsBackward,
        float forwardPos, float backwardPos,
        float spaceForward, float spaceBackward)
    {
        bool preferForward = _side switch
        {
            PreferredSide.Forward  => true,
            PreferredSide.Backward => false,
            _                      => spaceForward >= spaceBackward 
        };

        return preferForward
            ? (fitsForward  || !fitsBackward ? forwardPos  : backwardPos)
            : (fitsBackward || !fitsForward  ? backwardPos : forwardPos);
    }
    
    private static float ClampSafe(float value, float min, float max) =>
        Mathf.Clamp(value, min, Mathf.Max(min, max));
}