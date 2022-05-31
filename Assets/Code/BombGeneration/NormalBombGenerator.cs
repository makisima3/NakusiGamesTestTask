using System.Collections.Generic;
using System.Linq;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Plugins.RobyyUtils;
using UnityEngine.Events;

namespace Code.BombGeneration
{
    public class NormalBombGenerator : IBombGenerator
    {
        public List<IBomb> Generate(Cell[,] cells, int count)
        {
            var bombs = new List<IBomb>();
            var bombedCells = cells.Cast<Cell>().Where(cell => cell.Type == CellType.Empty).ChooseManyUnique(count);

            foreach (var cell in bombedCells)
            {
                
                var bomb = new NormalBomb()
                {
                    Damage = 2,
                    Radius = 2,
                    Position = cell.Position,
                    TimeToExplode = 3f
                };
                
                bombs.Add(bomb);
                
            }

            return bombs;
        }
    }
}