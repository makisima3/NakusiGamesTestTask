using System;
using System.Linq;
using Code.Enums;
using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.MapGeneration
{
    public class CellView : MonoBehaviour, IInitialized<CellViewInitData>
    {
        [Serializable]
        private class Pair
        {
            public CellType type;
            public GameObject view;
        }

        [SerializeField] private Pair[] _pairs;
       
        public void Initialize(CellViewInitData initData)
        {
           Instantiate(_pairs.First(p => p.type == initData.Cell.Type).view,transform);
        }
    }
}