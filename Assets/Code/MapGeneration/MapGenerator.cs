using System.Collections.Generic;
using System.Linq;
using Code.BombGeneration;
using Code.CharactersGeneration;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Plugins.SimpleFactory;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.MapGeneration
{
    public class MapGenerator
    {
        private Vector2Int _size;
        private int _minRoomBorder;
        private int _splitCount;
        private int _wayWidth;
        private IRoomSplitter _roomSplitter;
        private List<Room> _rooms;
        private Cell[,] _cells;
        private int _minWallSize;

        private readonly List<Vector2Int> _offsets = new()
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        public Cell[,] Cells => _cells;

        public MapGenerator(MapGeneratorSettings initData)
        {
            _rooms = new List<Room>();

            _size = initData.Size;
            _minRoomBorder = initData.MinRoomBorder;
            _splitCount = initData.SplitCount;
            _wayWidth = initData.WayWidth;
            _roomSplitter = initData.RoomSplitter;
            _minWallSize = initData.MinWallSize;
        }

        public void Generate()
        {
            FillCells();

            var initialRoom = new Room()
            {
                X = 0,
                Y = 0,
                Width = _size.x,
                Height = _size.y,
                MinBorder = _minRoomBorder,
                IsVertical = true,
            };

            SplitRooms(_splitCount, initialRoom);

            RemoveWalls();

            ClearSmallWalls();
        }

        private void SplitRooms(int counter, Room room)
        {
            if (counter > 0)
            {
                (var firstRoom, var secondRoom) = _roomSplitter.SplitUpRoom(room);

                SplitRooms(counter - 1, firstRoom);
                SplitRooms(counter - 1, secondRoom);
            }
            else
            {
                _rooms.Add(room);

                SetRoomWalls(room);
            }
        }

        private void SetRoomWalls(Room room)
        {
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    if (_cells[x, y].Type == CellType.Border)
                        continue;

                    if ((x == room.X || x == room.X + room.Width) ||
                        (y == room.Y || y == room.Y + room.Height))
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

        private void RemoveWalls(Room room, bool isVertical)
        {
            var bottomRange = isVertical ? room.Left + 1 : room.Bottom + 1;
            var topRange = isVertical ? room.Right - 1 : room.Top - 1;
            var constAxis = isVertical ? room.Top : room.Right;
            var randomAxis = Random.Range(bottomRange, topRange);

            var halfWayWidth = _wayWidth / 2;

            if (_wayWidth % 2 != 0)
            {
                halfWayWidth += 1;
            }

            var start = randomAxis - halfWayWidth;
            var end = randomAxis + _wayWidth / 2;

            for (var i = start; i < end; i++)
            {
                var x = isVertical ? i : constAxis;
                var y = isVertical ? constAxis : i;

                var cell = GetCell(x, y);

                if (cell != null && cell.Type != CellType.Border)
                    cell.SetType(CellType.Empty);
            }
        }

        private void RemoveWalls()
        {
            foreach (var room in _rooms)
            {
                RemoveWalls(room, true);
                RemoveWalls(room, false);
            }
        }

        private void ClearSmallWalls()
        {
            var visitedCells = new List<Cell>();
            var invalidGroups = _cells.Cast<Cell>()
                .Where(cell => cell.Type == CellType.Wall)
                .Select(cell => FindGroup(cell, visitedCells))
                .Where(group => group.Count < _minWallSize);

            foreach (var invalidGroup in invalidGroups)
            {
                invalidGroup.ForEach(cell => cell.SetType(CellType.Empty));
            }
        }

        private List<Cell> FindGroup(Cell currentCell, List<Cell> visitedCells)
        {
            var group = new List<Cell>();

            if (visitedCells.Contains(currentCell))
                return group;

            visitedCells.Add(currentCell);
            group.Add(currentCell);

            foreach (var offset in _offsets)
            {
                var nextCell = GetCell(currentCell.Position + offset);

                if (nextCell == null || nextCell.Type != CellType.Wall)
                    continue;

                group.AddRange(FindGroup(nextCell, visitedCells));
            }

            return group;
        }

        private Cell GetCell(Vector2Int position) => GetCell(position.x, position.y);

        private Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= _size.x)
                return null;

            if (y < 0 || y >= _size.y)
                return null;

            return _cells[x, y];
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