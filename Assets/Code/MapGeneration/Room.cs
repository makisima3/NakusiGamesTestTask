using System.Runtime.InteropServices;
using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public int Top => Y + Height;
        public int Right => X + Width;
        public int Bottom => Y;
        public int Left => X;
        
        public int MinBorder { get; set; }
        public bool IsVertical { get; set; }
    }
}