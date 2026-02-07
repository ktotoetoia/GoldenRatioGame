using UnityEngine;

namespace IM.Visuals
{
    public struct RowLayout
    {
        private Rect _r;
        private readonly float _line;
        private readonly float _pad;

        public RowLayout(Rect r, float line, float pad)
        {
            _r = r;
            _line = line;
            _pad = pad;
        }

        public Rect Next()
        {
            Rect current = new Rect(_r.x, _r.y, _r.width, _line);
            _r.y += _line + _pad;
            return current;
        }
    }
}