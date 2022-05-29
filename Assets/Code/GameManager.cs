using System;
using Code.Factories;
using Code.InitDatas;
using Code.MapGeneration;
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
            mapGenerator = new MapGenerator();
            mapGenerator.Initialize(new MapGeneratorInitData()
            {
                Size = size,
                SplitCount = splitCount,
                MinRoomBorder = minRoomBorder,
                WayWidth = wayWidth,
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