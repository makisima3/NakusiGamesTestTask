using System;
using System.Collections;
using System.Collections.Generic;
using Code.BombGeneration;
using Code.CharactersGeneration;
using Code.Factories;
using Code.InitDatas;
using Code.MapGeneration;
using Code.RoomSplitters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Code
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private MapView mapView;
        [SerializeField] private WorldFactory worldFactory;
        
        [Space]
        [SerializeField] private Vector2Int size;
        [SerializeField] private int splitCount = 3;
        [SerializeField] private int minRoomBorder = 1;
        [SerializeField] private int wayWidth = 5;
        
        [Space]
        [SerializeField] private int simpleCharactersCount = 10;

        [Space] 
        [SerializeField] private Grid grid;
        [SerializeField] private BombViewSpawner bombViewSpawner;
        [SerializeField] private int waveBombsCount = 5;
        [SerializeField] private float waveBombDelay = 4f;

        [Space] 
        [SerializeField] private CameraController cameraController;

        private MapGenerator _mapGenerator;
        private void Awake()
        {
            cameraController.SetPosition(size);
            
            _mapGenerator = new MapGenerator(new MapGeneratorSettings()
            {
                Size = size,
                SplitCount = splitCount,
                MinRoomBorder = minRoomBorder,
                WayWidth = wayWidth,
                RoomSplitter = new RandomAxisRoomSplitter(),
                MinWallSize = 4,
            });
            
            _mapGenerator.Generate();
            
            
            mapView.Initialize(new MapViewInitData()
            {
                Cells = _mapGenerator.Cells,
                WorldFactory = worldFactory
            });
           
            var charactersSpawner = new CharacterViewSpawner()
            {
                Characters =  new SimpleCharactersGenerator().Generate(_mapGenerator.Cells, simpleCharactersCount),
                Factory = worldFactory,
                Grid = grid,
            };
            charactersSpawner.Spawn(_mapGenerator.Cells);
            
            var bombsGenerator = new NormalBombGenerator();
            bombViewSpawner.Initialize(new BombViewSpawnerInitData()
            {
                Count = waveBombsCount,
                Factory = worldFactory,
                Grid = grid,
                BombGenerator = bombsGenerator,
                SpawnDelay = waveBombDelay,
                OnExplodeCallback = mapView.ExplodeCells
            });
            bombViewSpawner.Spawn(_mapGenerator.Cells);
        }
    }
}