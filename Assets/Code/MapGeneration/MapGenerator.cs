using System.Collections.Generic;
using System.Linq;
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
        private List<RoomHolder> _roomHolders;
        private Cell[,] _cells;
        private int _minWallSize;

        public Cell[,] Cells => _cells;

        public MapGenerator(MapGeneratorInitData initData)
        {
            _roomHolders = new List<RoomHolder>();

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
            var initialRoom = new RoomHolder(new RoomInitData()
            {
                X = 0,
                Y = 0,
                Width = _size.x,
                Height = _size.y,
                MinBorder = _minRoomBorder,
                IsVertical = true,
            });

            SplitRooms(_splitCount, initialRoom);

            RemoveWalls();

            ClearSmallWalls();
        }

        private void SplitRooms(int counter, RoomHolder room)
        {
            if (counter > 0)
            {
                (var firstRoom, var secondRoom) = _roomSplitter.SplitUpRoom(room);

                SplitRooms(counter - 1, firstRoom);
                SplitRooms(counter - 1, secondRoom);
            }
            else
            {
                _roomHolders.Add(room);

                SetRoomWalls(room);
            }
        }

        private void SetRoomWalls(RoomHolder room)
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

                for (int i = randomY - (_wayWidth / 2 + (_wayWidth % 2 == 0 ? 0 : 1)); i < randomY + _wayWidth / 2; i++)
                {
                    if (i < 0 || i >= _size.x)
                        continue;

                    leftCells.Add(_cells[x + width, i]);
                }

                for (int i = randomX - (_wayWidth / 2 + (_wayWidth % 2 == 0 ? 0 : 1)); i < randomX + _wayWidth / 2; i++)
                {
                    if (i < 0 || i >= _size.x)
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

        List<Vector2Int> offsets = new List<Vector2Int>()
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.one, -Vector2Int.one,
            new(1, -1), new(-1, 1)
        };
        
        List<Vector2Int> offsets2 = new List<Vector2Int>()
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

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
            
            foreach (var offset in offsets2)
            {
                var nextCell = GetCell(currentCell.Position + offset);
                
                if(nextCell == null || nextCell.Type != CellType.Wall)
                    continue;

                group.AddRange(FindGroup(nextCell, visitedCells));
            }

            return group;
        }

        private Cell GetCell(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _size.x)
                return null;

            if (position.y < 0 || position.y >= _size.y)
                return null;

            return _cells[position.x, position.y];
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