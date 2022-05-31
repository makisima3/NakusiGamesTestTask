using Code.BombGeneration;
using Code.Interfaces;
using UnityEngine;

namespace Code.InitDatas
{
    public class NormalBombViewInitData
    {
        public Vector3 FallPosition { get; set; }

        public IBomb Bomb { get; set; }
    }
}