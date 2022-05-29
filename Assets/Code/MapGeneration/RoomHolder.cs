using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    public class RoomHolder : IInitialized<RoomInitData>
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private int _minBorder;
        private bool _isVertical;
        private int _level;

        public int X => _x;
        public int Y => _y;
        public int Width => _width;
        public int Height => _height;
        public int MINBorder => _minBorder;
        public bool IsVertical => _isVertical;
        public int Level => _level;

        public void Initialize(RoomInitData initData)
        {
            _x = initData.X;
            _y = initData.Y;
            _width = initData.Width;
            _height = initData.Height;
            _minBorder = initData.MinBorder;
            _isVertical = initData.IsVertical;
            _level = initData.Level;
        }

        public (RoomHolder, RoomHolder) SplitUp()
        {
            _isVertical = !_isVertical;

            var axis = _isVertical ? _width : _height;
            var splitPoint = Random.Range(axis / 2 - axis / _minBorder, axis / 2 + axis / _minBorder);

            var xA = _x;
            var yA = _y;
            var widthA = _isVertical ? splitPoint : _width;
            var heightA = _isVertical ? _height : splitPoint;

            var xB = _isVertical ? _x + splitPoint : _x;
            var yB = _isVertical ? _y : _y + splitPoint;
            var widthB = _isVertical ? _width - splitPoint : _width;
            var heightB = _isVertical ? _height : _height - splitPoint;

            var roomA = new RoomHolder();
            roomA.Initialize(new RoomInitData()
            {
                X = xA,
                Y = yA,
                Width = widthA,
                Height = heightA,
                IsVertical = _isVertical,
                MinBorder = _minBorder,
                Level = _level,
            });
                
            var roomB = new RoomHolder();
            roomB.Initialize(new RoomInitData()
            {
                X = xB,
                Y = yB,
                Width = widthB,
                Height = heightB,
                IsVertical = _isVertical,
                MinBorder = _minBorder,
                Level = _level,
            });

            return (roomA, roomB);

/*
            if (_isVertical)
            {
                var splitPoint = Random.Range(_width / 2 - _width / _minBorder, _width / 2 + _width / _minBorder);

                var roomA = new RoomHolder();
                roomA.Initialize(new RoomInitData()
                {
                    X = _x,
                    Y = _y,
                    Width = splitPoint,
                    Height = _height,
                    MinBorder = _minBorder,
                    IsVertical = _isVertical,
                    Level = _level - 1,
                });

                var roomB = new RoomHolder();
                roomB.Initialize(new RoomInitData()
                {
                    X = _x + splitPoint,
                    Y = _y,
                    Width = _width - splitPoint,
                    Height = _height,
                    MinBorder = _minBorder,
                    IsVertical = _isVertical,
                    Level = _level - 1,
                });

                return (roomA, roomB);
            }
            else
            {
                var splitPoint = Random.Range(_height / 2 - _height / _minBorder, _height / 2 + _height / _minBorder);
                var roomA = new RoomHolder();
                roomA.Initialize(new RoomInitData()
                {
                    X = _x,
                    Y = _y,
                    Width = _width,
                    Height = splitPoint,
                    MinBorder = _minBorder,
                    IsVertical = _isVertical,
                    Level = _level - 1,
                });

                var roomB = new RoomHolder();
                roomB.Initialize(new RoomInitData()
                {
                    X = _x,
                    Y = _y + splitPoint,
                    Width = _width,
                    Height = _height - splitPoint,
                    MinBorder = _minBorder,
                    IsVertical = _isVertical,
                    Level = _level - 1,
                });

                return (roomA, roomB);
            }
            
            */

        }
    }
}