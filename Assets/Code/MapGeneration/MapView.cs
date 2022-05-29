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
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject ghost;

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

            //Prepare();
            SpawnCells();
        }

        private void Prepare()
        {
            var holder = Instantiate(container, transform);
            holder.name = $"RoomHolder{-1}";
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Instantiate(ghost, holder.transform);
                    ghost.transform.position = GetCellPosition(new Vector2Int(x, y));
                }
            }
        }

        private void SpawnCells()
        {
            foreach (var cell in _cells)
            {
                var color = cell.Type switch
                {
                    CellType.Border => Color.magenta,
                    CellType.Wall => Color.red,
                    _ => Color.black
                };

                var cellView = _worldFactory.Create<CellView, CellViewInitData>(new CellViewInitData()
                {
                    Color = color,
                });

                cellView.transform.SetParent(transform);
                cellView.transform.position = GetCellPosition(cell.Position);
            }

            /* var counter = 0;
 
             foreach (var roomHolder in _roomHolders)
             {
                 var holder = Instantiate(container, transform);
                 holder.name = $"RoomHolder{counter}";
                 Debug.Log(
                     $"holder #{counter} x:{roomHolder.X}|y:{roomHolder.Y}|width:{roomHolder.Width}|Height:{roomHolder.Height}");
 
                 spawnedHolders.Add(holder);
 
                 for (int x = roomHolder.X; x < roomHolder.X + roomHolder.Width; x++)
                 {
                     for (int y = roomHolder.Y; y < roomHolder.Y + roomHolder.Height; y++)
                     {
                         if ((x < roomHolder.X + roomHolder.MINBorder ||
                              x >= roomHolder.X + roomHolder.Width - roomHolder.MINBorder) ||
                             (y < roomHolder.Y + roomHolder.MINBorder ||
                              y >= roomHolder.Y + roomHolder.Height - roomHolder.MINBorder))
                         {
                             var cellView = _worldFactory.Create<CellView, CellViewInitData>(new CellViewInitData()
                             {
                                 Color = Color.red
                             });
 
                             cellView.transform.SetParent(holder.transform);
                             cellView.transform.position = GetCellPosition(new Vector2Int(x, y));
                         }
                         else
                         {
                             var cellView = _worldFactory.Create<CellView, CellViewInitData>(new CellViewInitData()
                             {
                                 Color = Color.black
                             });
 
                             cellView.transform.SetParent(holder.transform);
                             cellView.transform.position = GetCellPosition(new Vector2Int(x, y));
                         }
                     }
                 }
 
                 //yield return new WaitForEndOfFrame();
 
                 counter++;
             }*/
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