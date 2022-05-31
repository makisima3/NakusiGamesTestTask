using System;
using System.Collections;
using System.Collections.Generic;
using Code.Extensions;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Plugins.SimpleFactory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Code.BombGeneration
{
    public class BombViewSpawner : MonoBehaviour, ISpawner, IInitialized<BombViewSpawnerInitData>
    {
        public Grid Grid { get; private set; }

        public SimpleFactory Factory { get; private set; }
        public UnityAction<IBomb> OnExplodeCallback { get; private set; }

        private int _count;
        private float _spawnDelay;
        private bool _isSpawn;
        private IBombGenerator _generator;
        private Cell[,] _cells;

        public void Initialize(BombViewSpawnerInitData initData)
        {
            _count = initData.Count;
            Grid = initData.Grid;
            Factory = initData.Factory;
            OnExplodeCallback = initData.OnExplodeCallback;
            _spawnDelay = initData.SpawnDelay;
            _generator = initData.BombGenerator;
        }

        public void Spawn(Cell[,] cells)
        {
            _isSpawn = true;
            _cells = cells;

            StartCoroutine(SpawnWithDelay());
        }

        public void StopSpawn()
        {
            _isSpawn = false;
        }

        private IEnumerator SpawnWithDelay()
        {
            while (_isSpawn)
            {
                foreach (var bomb in _generator.Generate(_cells, _count))
                {
                    if (bomb is NormalBomb normalBomb)
                    {
                        var view = Factory.Create<NormalBombView, NormalBombViewInitData>(
                            new NormalBombViewInitData()
                            {
                                Bomb = bomb,
                                FallPosition = this.GetWorldPosition((Vector3Int) bomb.Position)
                            });
                        if (OnExplodeCallback != null)
                        {
                            view.OnExplode.AddListener(OnExplodeCallback);
                        }

                        view.Fire();
                    }
                }

                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}