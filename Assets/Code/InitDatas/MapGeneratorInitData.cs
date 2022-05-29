using Code.Factories;
using Code.Interfaces;
using UnityEngine;

namespace Code.InitDatas
{
    public class MapGeneratorInitData
    {
        public Vector2Int Size { get; set; }
        public int MinRoomBorder { get; set; }
        public int SplitCount { get; set; }
        public int WayWidth { get; set; }
        public IRoomSplitter RoomSplitter { get; set; }
    }
}