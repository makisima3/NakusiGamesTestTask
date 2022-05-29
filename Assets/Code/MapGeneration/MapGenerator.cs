using System.Collections.Generic;
using System.Linq;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using DefaultNamespace;
using Plugins.SimpleFactory;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.MapGeneration
{
    public class MapGenerator : IInitialized<MapGeneratorInitData>
    {
        private Vector2Int _size;
        private int _minRoomBorder;
        private int _splitCount;
        private int wayWidth;

        private List<RoomHolder> _roomHolders;
        private Cell[,] _cells;

        public List<RoomHolder> RoomHolders => _roomHolders;

        public Cell[,] Cells => _cells;

        public void Initialize(MapGeneratorInitData initData)
        {
            _roomHolders = new List<RoomHolder>();

            _size = initData.Size;
            _minRoomBorder = initData.MinRoomBorder;
            _splitCount = initData.SplitCount;
            wayWidth = initData.WayWidth;
        }

        public void Generate()
        {
            FillCells();
            var initialRoom = new RoomHolder(new RoomInitData()
            {
                X = 0,
                Y = 0,
                Width = _size.x,
                Height = _size.y,
                MinBorder = _minRoomBorder,
                IsVertical = true,
            });

            (var roomA, var roomB) = SplitUpRoom(initialRoom);

            SplitRooms(_splitCount - 1, roomA, roomB);

            RemoveWalls();
        }

        private void SplitRooms(int counter, RoomHolder roomA, RoomHolder roomB)
        {
            if (counter > 0)
            {
                (var firstRoomA, var secondRoomA) = SplitUpRoom(roomA);

                SplitRooms(counter - 1, firstRoomA, secondRoomA);

                (var firstRoomB, var secondRoomB) = SplitUpRoom(roomB);

                SplitRooms(counter - 1, firstRoomB, secondRoomB);
            }
            else
            {
                _roomHolders.Add(roomA);
                _roomHolders.Add(roomB);

                for (int x = roomA.X; x < roomA.X + roomA.Width; x++)
                {
                    for (int y = roomA.Y; y < roomA.Y + roomA.Height; y++)
                    {
                        if (_cells[x, y].Type == CellType.Border)
                            continue;

                        if ((x == roomA.X || x == roomA.X + roomA.Width) ||
                            (y == roomA.Y || y == roomA.Y + roomA.Height))
                        {
                            _cells[x, y].SetType(CellType.Wall);
                        }
                        else
                        {
                            _cells[x, y].SetType(CellType.Empty);
                        }
                    }
                }

                for (int x = roomB.X; x < roomB.X + roomB.Width; x++)
                {
                    for (int y = roomB.Y; y < roomB.Y + roomB.Height; y++)
                    {
                        if (_cells[x, y].Type == CellType.Border)
                            continue;

                        if ((x == roomB.X || x == roomB.X + roomB.Width) ||
                            (y == roomB.Y || y == roomB.Y + roomB.Height))
                        {
                            _cells[x, y].SetType(CellType.Wall);
                        }
                        else
                        {
                            _cells[x, y].SetType(CellType.Empty);
                        }
                    }
                }
            }
        }


        public (RoomHolder, RoomHolder) SplitUpRoom(RoomHolder room)
        {
            var isVertical = !room.IsVertical;
            
            var axis = isVertical ? room.Width : room.Height;
            var splitPoint = Random.Range(axis / 2 - axis / room.MINBorder, axis / 2 + axis / room.MINBorder);

            var xA = room.X;
            var yA = room.Y;
            var widthA = isVertical ? splitPoint : room.Width;
            var heightA = isVertical ? room.Height : splitPoint;

            var xB = isVertical ? room.X + splitPoint : room.X;
            var yB = isVertical ? room.Y : room.Y + splitPoint;
            var widthB = isVertical ? room.Width - splitPoint : room.Width;
            var heightB = isVertical ? room.Height : room.Height - splitPoint;

            var roomA = new RoomHolder(new RoomInitData()
            {
                X = xA,
                Y = yA,
                Width = widthA,
                Height = heightA,
                IsVertical = isVertical,
                MinBorder = room.MINBorder,
            });
                
            var roomB = new RoomHolder(new RoomInitData()
            {
                X = xB,
                Y = yB,
                Width = widthB,
                Height = heightB,
                IsVertical = isVertical,
                MinBorder = room.MINBorder,
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

        
        private void RemoveWalls()
        {
            var counter = 0;
            foreach (var holder in _roomHolders)
            {
                counter++;

                var x = holder.X + 1;
                var y = holder.Y + 1;

                var width = holder.Width - 1;
                var height = holder.Height - 1;

                var randomX = Random.Range(x, x + width);
                var randomY = Random.Range(y, y + height);

                if (x + width >= 100)
                    width -= 1;
                if (y + height >= 100)
                    height -= 1;

                var leftCells = new List<Cell>();
                var topCells = new List<Cell>();

                for (int i = randomY - (wayWidth / 2 + (wayWidth%2 == 0? 0: 1)) ; i < randomY + wayWidth / 2; i++)
                {
                    if(i < 0 || i >= _size.x)
                        continue;
                    
                    leftCells.Add(_cells[x + width, i]);
                }

                for (int i = randomX - (wayWidth / 2 + (wayWidth%2 == 0? 0: 1)); i < randomX + wayWidth / 2; i++)
                {
                    if(i < 0 || i >= _size.x)
                        continue;
                    
                    leftCells.Add(_cells[i, y + height]);
                }


                foreach (var leftCell in leftCells)
                {
                    if (leftCell.Type != CellType.Border)
                        leftCell.SetType(CellType.Empty);
                }

                foreach (var topCell in topCells)
                {
                    if (topCell.Type != CellType.Border)
                        topCell.SetType(CellType.Empty);
                }
            }
        }
        
        private void FillCells()
        {
            _cells = new Cell[_size.x, _size.y];

            var type = CellType.None;

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    var cell = new Cell();

                    if ((x == 0 || x == _size.x - 1) || (y == 0 || y == _size.y - 1))
                    {
                        type = CellType.Border;
                    }
                    else
                    {
                        type = CellType.Empty;
                    }

                    cell.Initialize(new CellInitData()
                    {
                        Type = type,
                        Position = new Vector2Int(x, y)
                    });
                    _cells[x, y] = cell;
                }
            }
        }
    }
}