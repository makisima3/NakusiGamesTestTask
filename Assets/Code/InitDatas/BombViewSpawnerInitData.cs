using System.Collections.Generic;
using Code.BombGeneration;
using Code.Interfaces;
using Plugins.SimpleFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Code.InitDatas
{
    public class BombViewSpawnerInitData
    {
        public int Count { get; set;}
        public Grid Grid { get; set; }
        public float SpawnDelay { get; set; }
        public SimpleFactory Factory { get; set; }
        public UnityAction<IBomb> OnExplodeCallback { get; set; }
        public IBombGenerator BombGenerator;
    }
}