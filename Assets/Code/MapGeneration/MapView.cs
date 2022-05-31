using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Code.BombGeneration;
using Code.CharactersGeneration;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    [RequireComponent(typeof(Grid))]
    public class MapView : MonoBehaviour, IInitialized<MapViewInitData>
    {
        [SerializeField] private Transform container;

        private WorldFactory _worldFactory;
        private Cell[,] _cells;
        private List<GameObject> spawnedHolders;
        private Grid _grid;

        private readonly List<Vector2Int> _axisOffsets = new()
        {
            Vector2Int.up,Vector2Int.right,Vector2Int.left,  Vector2Int.down, 
        };

        public void Initialize(MapViewInitData initData)
        {
            _worldFactory = initData.WorldFactory;
            _cells = initData.Cells;

            _grid = GetComponent<Grid>();
            ClearHolders();

            SpawnCells();
        }

        public void ExplodeCells(IBomb bomb)
        {
            var characters = new HashSet<IDamageable>();
            var blockedOffsets = new List<Vector2Int>();

            for (int i = 0; i < bomb.Radius; i++)
            {
                foreach (var offset in _axisOffsets)
                {
                    if (blockedOffsets.Contains(offset))
                        continue;

                    var pos = bomb.Position + offset * i;
                    var cell = _cells[pos.x, pos.y];

                    if (cell.Type == CellType.Wall)
                    {
                        blockedOffsets.Add(offset);
                        continue;
                    }

                    if (cell.HasDamageable)
                    {
                        characters.Add(cell.Damageable);
                    }
                }
            }

            foreach (var character in characters)
            {
                character.TakeDamage(bomb.Damage);
            }
        }

        private void SpawnCells()
        {
            foreach (var cell in _cells)
            {
                var cellView = _worldFactory.Create<CellView, CellViewInitData>(new CellViewInitData()
                {
                    Cell = cell,
                });

                cellView.transform.SetParent(container);
                cellView.transform.position = GetCellPosition(cell.Position);
            }
        }

        private void ClearHolders()
        {
            spawnedHolders ??= new List<GameObject>();
            foreach (var holder in spawnedHolders)
            {
                Destroy(holder.gameObject);
            }

            spawnedHolders.Clear();
            spawnedHolders = new List<GameObject>();
        }


        private Vector3 GetCellPosition(Vector2Int pos)
        {
            return _grid.CellToWorld((Vector3Int) pos);
        }
    }
}