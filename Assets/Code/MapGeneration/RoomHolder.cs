using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    public class RoomHolder
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private int _minBorder;
        private bool _isVertical;

        public int X => _x;
        public int Y => _y;
        public int Width => _width;
        public int Height => _height;

        public int MINBorder => _minBorder;

        public bool IsVertical => _isVertical;

        public RoomHolder(RoomInitData initData)
        {
            _x = initData.X;
            _y = initData.Y;
            _width = initData.Width;
            _height = initData.Height;
            _minBorder = initData.MinBorder;
            _isVertical = initData.IsVertical;
        }
    }
}