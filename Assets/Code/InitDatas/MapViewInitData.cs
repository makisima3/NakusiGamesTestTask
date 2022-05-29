using System.Collections.Generic;
using Code.Factories;
using Code.MapGeneration;
using DefaultNamespace;

namespace Code.InitDatas
{
    public class MapViewInitData
    {
        public WorldFactory WorldFactory { get; set; }
        public Cell[,] Cells{ get; set; }
    }
}