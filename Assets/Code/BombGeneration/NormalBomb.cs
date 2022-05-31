using Code.InitDatas;
using Code.Interfaces;
using Plugins.SimpleFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Code.BombGeneration
{
    public class NormalBomb : IBomb
    {
        public Vector2Int Position { get; set; }
        public int Damage { get; set; }
        public int Radius { get; set; }
        
        public float TimeToExplode { get; set; }
    }
}