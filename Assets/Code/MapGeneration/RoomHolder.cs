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

        public (RoomHolder,RoomHolder) SplitUp()
        {
            _isVertical = !_isVertical;
            if (_isVertical)
            {
                var splitPoint = Random.Range(_width/ 2 - _width /5, _width/2 + _width / 5);
                
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
                var splitPoint = Random.Range(_height/ 2 - _height /5, _height/2 + _height / 5);
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
        }
    }
}