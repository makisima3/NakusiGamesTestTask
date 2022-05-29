using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using DefaultNamespace;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    [RequireComponent(typeof(Grid))]
    public class MapView : MonoBehaviour, IInitialized<MapViewInitData>
    {
        [SerializeField] private float spawnDelay = 0.1f;
        [SerializeField] private Transform container;

        private WorldFactory _worldFactory;
        private Cell[,] _cells;
        private List<GameObject> spawnedHolders;
        private Grid _grid;

        public void Initialize(MapViewInitData initData)
        {
            _worldFactory = initData.WorldFactory;
            _cells = initData.Cells;

            _grid = GetComponent<Grid>();
            ClearHolders();
            
            SpawnCells();
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