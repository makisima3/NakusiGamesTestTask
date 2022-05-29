using System;
using Code.Factories;
using Code.InitDatas;
using Code.MapGeneration;
using Code.RoomSplitters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapView mapView;
        [SerializeField] private WorldFactory worldFactory;
        
        [SerializeField] private Vector2Int size;
        [SerializeField] private int splitCount = 3;
        [SerializeField] private int minRoomBorder = 1;
        [SerializeField] private int wayWidth = 5;
        
        private MapGenerator mapGenerator;

        private void Awake()
        {
            mapGenerator = new MapGenerator(new MapGeneratorInitData()
            {
                Size = size,
                SplitCount = splitCount,
                MinRoomBorder = minRoomBorder,
                WayWidth = wayWidth,
                RoomSplitter = new RandomAxisRoomSplitter()
            });
            
            mapGenerator.Generate();
            
            mapView.Initialize(new MapViewInitData()
            {
                Cells = mapGenerator.Cells,
                WorldFactory = worldFactory
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}